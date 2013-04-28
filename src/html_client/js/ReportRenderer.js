var ReportRenderer = Class.extend({
	execute: function (executionId) {
		var url = "/Prompts.Service/ReportViewer.aspx?ExecutionId=" + executionId;
		window.open(url,'_blank', "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no");
	}
});