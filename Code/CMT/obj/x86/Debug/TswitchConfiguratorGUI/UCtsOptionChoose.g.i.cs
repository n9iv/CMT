﻿#pragma checksum "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "13CCEBEA5DF5D9F42389EBD45A1C18A5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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


namespace CMT.TswitchConfiguratorGUI {
    
    
    /// <summary>
    /// UCtsOptionChoose
    /// </summary>
    public partial class UCtsOptionChoose : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbaaa;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton _rbbbbb;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel _sp;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox _cbMN;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
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
            System.Uri resourceLocater = new System.Uri("/CMT;component/tswitchconfiguratorgui/uctsoptionchoose.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
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
            this._rbaaa = ((System.Windows.Controls.RadioButton)(target));
            
            #line 12 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
            this._rbaaa.Checked += new System.Windows.RoutedEventHandler(this._rbaaa_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            this._rbbbbb = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
            this._rbbbbb.Checked += new System.Windows.RoutedEventHandler(this._rbbbbb_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this._sp = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this._cbMN = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
            this._cbMN.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this._cbMN_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this._cbBN = ((System.Windows.Controls.ComboBox)(target));
            
            #line 19 "..\..\..\..\TswitchConfiguratorGUI\UCtsOptionChoose.xaml"
            this._cbBN.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this._cbBN_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

