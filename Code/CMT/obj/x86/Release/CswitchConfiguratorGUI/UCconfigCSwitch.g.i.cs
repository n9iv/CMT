﻿#pragma checksum "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BF2FAA534C705AC9D82AD7C55C54DE04"
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


namespace CMT.CswitchConfiguratorGUI {
    
    
    /// <summary>
    /// UCconfigCSwitch
    /// </summary>
    public partial class UCconfigCSwitch : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox _chkbReset;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _btnConfig;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock _tbConf;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock _tbPb;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.UserControl _ucPb;
        
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
            System.Uri resourceLocater = new System.Uri("/CMT;component/cswitchconfiguratorgui/ucconfigcswitch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
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
            this._chkbReset = ((System.Windows.Controls.CheckBox)(target));
            
            #line 11 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
            this._chkbReset.Checked += new System.Windows.RoutedEventHandler(this._chkbReset_Checked);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
            this._chkbReset.Unchecked += new System.Windows.RoutedEventHandler(this._chkbReset_Unchecked);
            
            #line default
            #line hidden
            return;
            case 2:
            this._btnConfig = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\..\CswitchConfiguratorGUI\UCconfigCSwitch.xaml"
            this._btnConfig.Click += new System.Windows.RoutedEventHandler(this._btnConfig_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this._tbConf = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this._tbPb = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this._ucPb = ((System.Windows.Controls.UserControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

