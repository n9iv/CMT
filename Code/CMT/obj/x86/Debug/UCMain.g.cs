﻿#pragma checksum "..\..\..\UCMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AD94BE836200E6176C7DB93BFFBE5AB6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CMT {
    
    
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel _sp;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbClockConf;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbTswitchConf;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbCswitchConf;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbClientSN;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\UCMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image _iImageMain;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CMT;component/ucmain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UCMain.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this._sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this._rbClockConf = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\UCMain.xaml"
            this._rbClockConf.Checked += new System.Windows.RoutedEventHandler(this._rbClockConf_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this._rbTswitchConf = ((System.Windows.Controls.RadioButton)(target));
            
            #line 14 "..\..\..\UCMain.xaml"
            this._rbTswitchConf.Checked += new System.Windows.RoutedEventHandler(this._rbTswitchConf_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this._rbCswitchConf = ((System.Windows.Controls.RadioButton)(target));
            
            #line 15 "..\..\..\UCMain.xaml"
            this._rbCswitchConf.Checked += new System.Windows.RoutedEventHandler(this._rbCswitchConf_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this._rbClientSN = ((System.Windows.Controls.RadioButton)(target));
            
            #line 16 "..\..\..\UCMain.xaml"
            this._rbClientSN.Checked += new System.Windows.RoutedEventHandler(this._rbClientSN_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this._iImageMain = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

