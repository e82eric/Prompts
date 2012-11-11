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
using System;
using Prompts.Prompting.ViewModels;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class PromptBuilder:IPromptBuilder
    {
        private readonly IPromptBuilder _treeBuilder;
        private readonly IPromptBuilder _dropDownBuilder;
        private readonly IPromptBuilder _shoppingCartBuilder;
        private readonly IPromptBuilder _casscadingSearchShoppingCartBuilder;
        private readonly IPromptBuilder _singleSelectTreeBuilder;
        private readonly IPromptBuilder _emptyPromptBuilder;
        private readonly IPromptBuilder _recursiveTreeBuilder;
        private readonly IPromptBuilder _recursiveSingleSelectTreeBuilder;

        public PromptBuilder(
            IPromptBuilder treeBuilder
            , IPromptBuilder dropDownBuilder
            , IPromptBuilder shoppingCartBuilder
            , IPromptBuilder casscadingSearchShoppingCartBuilder
            , IPromptBuilder singleSelectTreeBuilder
            , IPromptBuilder emptyPromptBuilder
            , IPromptBuilder recursiveTreeBuilder
            , IPromptBuilder recursiveSingleSelectTreeBuilder)
        {
            _recursiveSingleSelectTreeBuilder = recursiveSingleSelectTreeBuilder;
            _recursiveTreeBuilder = recursiveTreeBuilder;
            _singleSelectTreeBuilder = singleSelectTreeBuilder;
            _emptyPromptBuilder = emptyPromptBuilder;
            _casscadingSearchShoppingCartBuilder = casscadingSearchShoppingCartBuilder;
            _shoppingCartBuilder = shoppingCartBuilder;
            _dropDownBuilder = dropDownBuilder;
            _treeBuilder = treeBuilder;
        }

        public IPrompt BuildFrom(PromptInfo promptInfo)
        {
            switch (promptInfo.PromptType)
            {
                case PromptType.Tree:
                    return _treeBuilder.BuildFrom(promptInfo);
                case PromptType.ShoppingCart:
                    return _shoppingCartBuilder.BuildFrom(promptInfo);
                case PromptType.DropDown:
                    return _dropDownBuilder.BuildFrom(promptInfo);
                case PromptType.CasscadingSearch:
                    return _casscadingSearchShoppingCartBuilder.BuildFrom(promptInfo);
                case PromptType.SingleSelectTree:
                    return _singleSelectTreeBuilder.BuildFrom(promptInfo);
                case PromptType.Empty:
                    return _emptyPromptBuilder.BuildFrom(promptInfo);
                case PromptType.RecursiveTree:
                    return _recursiveTreeBuilder.BuildFrom(promptInfo);
                case PromptType.RecursiveSingleSelectTree:
                    return _recursiveSingleSelectTreeBuilder.BuildFrom(promptInfo);
                default:
                    throw new Exception();
            }
        }
    }
}
