﻿#pragma checksum "..\..\Admin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "13991E429DF92460E8922B4F133E353D"
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
    /// Admin
    /// </summary>
    public partial class Admin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 46 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgMainWin;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnBnetwork;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnClock;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnEthSc;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnEthSs;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton btnNext;
        
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
            System.Uri resourceLocater = new System.Uri("/CMT;component/admin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Admin.xaml"
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
            this.imgMainWin = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.btnBnetwork = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 53 "..\..\Admin.xaml"
            this.btnBnetwork.Checked += new System.Windows.RoutedEventHandler(this.btnBnetwork_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnClock = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 54 "..\..\Admin.xaml"
            this.btnClock.Checked += new System.Windows.RoutedEventHandler(this.btnClock_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnEthSc = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 55 "..\..\Admin.xaml"
            this.btnEthSc.Checked += new System.Windows.RoutedEventHandler(this.btnEthSc_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnEthSs = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 56 "..\..\Admin.xaml"
            this.btnEthSs.Checked += new System.Windows.RoutedEventHandler(this.btnEthSs_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnNext = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 58 "..\..\Admin.xaml"
            this.btnNext.Click += new System.Windows.RoutedEventHandler(this.btnNext_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

