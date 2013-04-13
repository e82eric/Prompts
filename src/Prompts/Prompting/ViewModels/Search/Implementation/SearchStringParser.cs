// Copyright (c) 2010. Eric Nelson  
// https://code.google.com/p/prompts/
// All rights reserved.
//     
// Redistribution and use in source and binary forms,   
// with or without modification, are permitted provided   
// that the following conditions are met:    
// * Redistributions of source code must retain the   
// above copyright notice, this list of conditions and   
// the following disclaimer.    
// * Redistributions in binary form must reproduce   
// the above copyright notice, this list of conditions   
// and the following disclaimer in the documentation   
// and/or other materials provided with the distribution.    
// * Neither the name of Eric Nelson nor the   
// names of its contributors may be used to endorse   
// or promote products derived from this software   
// without specific prior written permission.    
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND   
// CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,   
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF   
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE   
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR   
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,   
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,   
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR   
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS   
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,   
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING   
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE   
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF   
// SUCH DAMAGE.
// 
//     
// [This is the BSD license, see  
// http://www.opensource.org/licenses/bsd-license.php]  
namespace Prompts.Prompting.ViewModels.Search.Implementation
{
    public class SearchStringParser<T> : ISearchStringParser<T>
    {
        private readonly ISearchProvider<T> _searchProvider;
        private readonly string _specialCharacter;

        public SearchStringParser(string specialCharacter, ISearchProvider<T> searchProvider)
        {
            _specialCharacter = specialCharacter;
            _searchProvider = searchProvider;
        }

        public T Parse(string searchExpression)
        {
            string parsedValue;

            if (searchExpression.Replace("*", string.Empty).Equals(string.Empty))
            {
                return _searchProvider.CreateNullSearch();
            }
            if (searchExpression.StartsWith(_specialCharacter) && searchExpression.EndsWith(_specialCharacter))
            {
                parsedValue = searchExpression.Substring(1, searchExpression.Length - 2);
                return _searchProvider.CreateContainsSearch(parsedValue);
            }
            if (searchExpression.EndsWith(_specialCharacter))
            {
                parsedValue = searchExpression.Substring(0, searchExpression.Length - 1);
                return _searchProvider.CreateStartsWithSearch(parsedValue);
            }
            if (searchExpression.StartsWith(_specialCharacter))
            {
                parsedValue = searchExpression.Substring(1, searchExpression.Length - 1);
                return _searchProvider.CreateEndsWithSearch(parsedValue);
            }

            return _searchProvider.CreateEqualsSearch(searchExpression);
        }
    }
}