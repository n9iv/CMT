﻿#pragma checksum "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6408DE9A55DEB6F27A7B57181806733F"
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
    /// UCclockConfigurator
    /// </summary>
    public partial class UCclockConfigurator : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbMFU;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbCCU;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel _sp;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox _tbBatVal;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel _spBtn;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Button _cmdUp;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.Button _cmdDown;
        
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
            System.Uri resourceLocater = new System.Uri("/CMT;component/clockconfiguratorgui/ucclockconfigurator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
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
            this._rbMFU = ((System.Windows.Controls.RadioButton)(target));
            
            #line 12 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
            this._rbMFU.Checked += new System.Windows.RoutedEventHandler(this._rbMFU_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this._rbCCU = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
            this._rbCCU.Checked += new System.Windows.RoutedEventHandler(this._rbCCU_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this._sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this._tbBatVal = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
            this._tbBatVal.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this._tbBatVal_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this._spBtn = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this._cmdUp = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
            this._cmdUp.Click += new System.Windows.RoutedEventHandler(this._cmdUp_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this._cmdDown = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\ClockConfiguratorGUI\UCclockConfigurator.xaml"
            this._cmdDown.Click += new System.Windows.RoutedEventHandler(this._cmdDown_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

