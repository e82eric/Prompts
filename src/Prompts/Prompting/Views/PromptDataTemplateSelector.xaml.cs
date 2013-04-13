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
using System.Windows;
using System.Windows.Controls;

namespace Prompts.Prompting.Views
{
    public partial class PromptDataTemplateSelector
    {
        private readonly PromptViewProvider _promptViewProvider;

        public PromptDataTemplateSelector()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            _promptViewProvider = new PromptViewProvider(
                new DelegatePromptViewProvider(() => FormatStretch(new ShoppingCartTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new ShoppingCartView())),
                new DelegatePromptViewProvider(() => FormatStretch(new CasscadingSearchView())),
                new DelegatePromptViewProvider(() => FormatDropDown(new DropDownView())),
                new DelegatePromptViewProvider(() => FormatDropDown(new SingleSelectTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new EmptyPromptView())));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Content = _promptViewProvider.Get(DataContext);
            Loaded -= OnLoaded;
        }

        private static UserControl FormatStretch(UserControl control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            return control;
        }

        private static UserControl FormatDropDown(UserControl control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Width = 300;
            control.Margin = new Thickness(5);
            return control;
        }
    }
}