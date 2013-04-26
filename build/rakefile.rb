require 'albacore'
require 'erb'
include ERB::Util

versionNumber = "1.0.0"
buildNumber = ENV['buildNumber'] || "0"

rootDirectory=".."
trunkDirectory = "#{rootDirectory}"
rootBuildDirectory = "#{trunkDirectory}/binaries"
solutionRoot = "#{trunkDirectory}/src"

task :default => [
	'Clean',
	'set_common_assembly_info',
	 'run_service_unit_tests',
	 'build_service',
	 'run_silverlight_unit_tests',
	 'build_web',
	 'build_silverlight_assembly',
	 'build_installer',
	 'add_documentation',
	 'build_sample_reports',
	 'compress_binaries',
	 'compress_sources']

desc "Clean"
task :Clean do
	FileUtils.remove_dir(rootBuildDirectory, true)
end

desc "SetCommonAssemblyInfo"
task :set_common_assembly_info do
	commonAssemblyInfo = "#{solutionRoot}/CommonAssemblyInfo.cs"

	text = File.read(commonAssemblyInfo)
	replace = text.gsub(/AssemblyVersion\(.*\)/, "AssemblyVersion(\"#{versionNumber}.#{buildNumber}\")")
	replace = replace.gsub(/AssemblyFileVersion\(.*\)/, "AssemblyFileVersion(\"#{versionNumber}.#{buildNumber}\")")
	File.open(commonAssemblyInfo, "w") {|file| file.puts replace}
end

desc "Run Service Unit Tests"
nunit :run_service_unit_tests do |nunit|
	testServiceName = "Test.Prompts.Service"
	testServiceProjectDirectory = "#{solutionRoot}/#{testServiceName}"

	build_project("#{testServiceProjectDirectory}/#{testServiceName}.csproj")
	
	nunit.command = "#{trunkDirectory}/tools/nunit/nunit-console.exe"
	nunit.options "/xml=#{testServiceProjectDirectory}/TestResults.xml"
	nunit.assemblies << "#{solutionRoot}/#{testServiceName}/bin/Release/#{testServiceName}.dll"
end

desc "Build Service"
task :build_service do
	serviceName = "Prompts.Service"
	serviceBuildDirectory = "#{rootBuildDirectory}/#{serviceName}"
	serviceProjectDirectory = "#{solutionRoot}/#{serviceName}"

	publish_web_project(serviceProjectDirectory, serviceName, serviceBuildDirectory)
end

desc "Run Silverlight Unit Tests"
exec :run_silverlight_unit_tests do |cmd|
	silverlightUnitTestName = "Test.Prompts"
	silverlightUnitTestProjectDirectory = "#{solutionRoot}/#{silverlightUnitTestName}"
	silverlightUnitTestBinDirectory = "#{silverlightUnitTestProjectDirectory}/Bin/Release"
	silverlightUnitTestAssembly = "#{silverlightUnitTestBinDirectory}/#{silverlightUnitTestName}.xap"
	
	build_project("#{silverlightUnitTestProjectDirectory}/#{silverlightUnitTestName}.csproj")
	
	cmd.command = "#{trunkDirectory}/tools/statlight/StatLight.exe"
	cmd.parameters "-x=\"#{silverlightUnitTestAssembly}\" -r\"#{silverlightUnitTestProjectDirectory}/TestResults.xml\""
end

desc "Build Web Project"
task :build_web do
	webName = "Prompts.Web"
	webProjectDirectory = "#{solutionRoot}/#{webName}"
	webBuildDirectory = "#{rootBuildDirectory}/#{webName}"
	
	publish_web_project(webProjectDirectory, webName, webBuildDirectory)
end

desc "Build Assembly"
task :build_silverlight_assembly do
	assemblyName = "Prompts"
	assemblyProjectDirectory = "#{solutionRoot}/Prompts"
	
	build_project("#{assemblyProjectDirectory}/#{assemblyName}.csproj")

	copy "#{assemblyProjectDirectory}/bin/Release/#{assemblyName}.dll", rootBuildDirectory
end

desc "Build Sample Reports"
task :build_sample_reports do
	reportDirectory = "#{trunkDirectory}/Sample Reports"

	Dir.glob "#{reportDirectory}/**/{*.rdl,*.rds,*.sln,*.rptproj}" do |path|
		newPath = path.gsub(reportDirectory, "#{rootBuildDirectory}/Sample Reports")
		FileUtils.mkdir_p File.dirname(newPath)
		copy path, newPath
	end
end

desc "Build Installer"
task :build_installer do
	installerName = "Prompts.Installer"
	installerProjectDirectory = "#{solutionRoot}/#{installerName}"
	
	build_project("#{installerProjectDirectory}/#{installerName}.wixproj")

	copy "#{installerProjectDirectory}/bin/Release/Prompts.msi", rootBuildDirectory
end

desc "Compress Binaries"
zip :compress_binaries do |zip|
     zip.directories_to_zip rootBuildDirectory
     zip.output_file = "prompts_#{versionNumber}.#{buildNumber}.zip"
     zip.output_path = rootDirectory
end

desc "Compress Sources"
zip :compress_sources do |zip|
     zip.directories_to_zip rootBuildDirectory, "#{trunkDirectory}"
	 zip.exclusions = [
		/_ReSharper/, 
		/.user/, 
		/.suo/, 
		/.resharper/, 
		/.cache/, 
		/.zip/, 
		/.orig/, 
		/Binaries/, 
		/TeestResults.xml/, 
		/bin/, 
		/Bin/, 
		/obj/, 
		/ClientBin/]
     zip.output_file = "prompts_#{versionNumber}.#{buildNumber}_src.zip"
     zip.output_path = rootDirectory
end

desc "Add Documentation"
task :add_documentation do
	docsDirectory = "#{trunkDirectory}/docs"
	Dir.glob "#{docsDirectory}/*" do |path|
		newPath = path.gsub(docsDirectory, rootBuildDirectory)
		copy path, newPath
	end
end

desc "Build Html Clients"
task :build_html_clients => ['Clean'] do
	builder = Html_Client_Builder.new "#{solutionRoot}/html_client", rootBuildDirectory
	builder.build_concatenated
	builder.build_debug
end

def publish_web_project(projectDirectory, projectName, buildDirectory)
	build_project("#{projectDirectory}/#{projectName}.csproj") 

	Dir.glob "#{projectDirectory}/{**/*.svc,bin/*.dll,**/*.aspx,/**/*.xap,Silverlight.js,web.config,*.asax}" do |path|
		newPath = path.gsub(projectDirectory, buildDirectory)
		FileUtils.mkdir_p File.dirname(newPath)
		copy path, newPath
	end
end

def build_project(projectFile)
	msbuild = MSBuild.new()
	msbuild.properties :configuration => :Release
	msbuild.targets :rebuild
	msbuild.build_solution(projectFile)
end

class Html_Client_Builder
	def initialize(sourceDirectory, binariesDirectory)
		@sourceDirectory = sourceDirectory
		@binariesDirectory = binariesDirectory
		@jsSourceDirectory = "#{@sourceDirectory}/js"
		@htmlSourceDirectory = "#{@sourceDirectory}/html"

		init_jsFiles
		init_templates
	end

	def build_concatenated
		build_html_client false
	end

	def build_debug
		build_html_client true
	end

	private

	def init_jsFiles
		@jsFiles = [
			"TemplateView.js",
			"AsynchronousItemsController.js", 
			"LoadingPanelControllerBase.js", 
			"PromptController.js", 
			"PromptView.js", 
			"ItemsView.js", 
			"SelectableItemController.js",
			"ExpandableItemController.js",
			"MultiSelectPromptController.js",
			"SingleSelectPromptController.js",
			"DropDownView.js",
			"ShoppingCartView.js"]
		
		Dir.chdir(@jsSourceDirectory) do
			Dir.glob "*.js" do |fileName|
				if !@jsFiles.include? fileName
					@jsFiles.push fileName
				end
			end
		end
	end

	def init_templates
		@templates = []
		
		Dir.chdir("#{@sourceDirectory}/templates") do
			Dir.glob "*.html" do |fileName|
				file = File.open(fileName)
				text = file.read
				obj = OpenStruct.new(:name => File.basename(fileName, ".*"), :text => text)
				@templates.push obj
			end
		end
	end

	def build_html_client(is_Debug)
		is_Debug ? outDir = "#{@binariesDirectory}/html_client_debug" : outDir = "#{@binariesDirectory}/html_client"

		jsOutDir = "#{outDir}/js"

		FileUtils.mkdir_p jsOutDir
		
		if is_Debug
			Dir.glob "#{@jsSourceDirectory}/{*.js}" do |path|
				copy path, jsOutDir
			end
			create_index_html "#{outDir}/html", @jsFiles
		else
			outFile = File.new("#{jsOutDir}/prompts.js", "w+")
			@jsFiles.each { |filePath| outFile.puts File.read("#{@jsSourceDirectory}/#{filePath}") }
			create_index_html "#{outDir}/html", ["prompts.js"]
		end
		
		cssOutDir = "#{outDir}/css"
		FileUtils.mkdir_p cssOutDir
		system("sass '#{@sourceDirectory}/css/prompts.scss':'#{cssOutDir}/prompts.css' --no-cache")
		FileUtils.cp_r "#{@sourceDirectory}/images", "#{outDir}/images"
		Dir.glob "#{@jsSourceDirectory}/external/{*}" do |path|
			copy path, jsOutDir
		end
	end

	def create_index_html(outputDirectory, jsFiles)
		FileUtils.mkdir_p outputDirectory
		template = ERB.new File.new("#{@htmlSourceDirectory}/index.html.erb").read, 0, ">"

		File.open("#{outputDirectory}/index.html", "w+") do |file|
		  file.puts template.result(binding)
		end
	end
end
