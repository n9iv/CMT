<<<<<<< HEAD
﻿#pragma checksum "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3FBC89BDD813D56B3AF1921F403F0F73"
=======
﻿#pragma checksum "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6D4010B005B97646800BFE0B53EBC6EA"
>>>>>>> origin/master
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
    /// UCcSwitchconfigOptions
    /// </summary>
    public partial class UCcSwitchconfigOptions : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbMain;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbRed;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel _sp;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox _cbBN;
        
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
            System.Uri resourceLocater = new System.Uri("/CMT;component/cswitchconfiguratorgui/uccswitchconfigoptions.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
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
            this._rbMain = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
            this._rbMain.Checked += new System.Windows.RoutedEventHandler(this._rbMain_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this._rbRed = ((System.Windows.Controls.RadioButton)(target));
            
            #line 14 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
            this._rbRed.Checked += new System.Windows.RoutedEventHandler(this._rbRed_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this._sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this._cbBN = ((System.Windows.Controls.ComboBox)(target));
            
            #line 17 "..\..\..\..\CswitchConfiguratorGUI\UCcSwitchconfigOptions.xaml"
            this._cbBN.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this._cbBN_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

