﻿#pragma checksum "..\..\QueryConstructorWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0A4DDAD20317BC2B983A85B1E5D0CAD8C3613CA5"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
using second_course;


namespace second_course {
    
    
    /// <summary>
    /// QueryConstructorWindow
    /// </summary>
    public partial class QueryConstructorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle HeaderQueryRectangle;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockQueryHeader;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ButtonQueryClose;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewTest;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridColumns;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\QueryConstructorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridResult;
        
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
            System.Uri resourceLocater = new System.Uri("/second_course;component/queryconstructorwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\QueryConstructorWindow.xaml"
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
            this.HeaderQueryRectangle = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 25 "..\..\QueryConstructorWindow.xaml"
            this.HeaderQueryRectangle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.HeaderQueryRectangle_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBlockQueryHeader = ((System.Windows.Controls.TextBlock)(target));
            
            #line 26 "..\..\QueryConstructorWindow.xaml"
            this.TextBlockQueryHeader.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.HeaderQueryRectangle_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ButtonQueryClose = ((System.Windows.Controls.Image)(target));
            
            #line 27 "..\..\QueryConstructorWindow.xaml"
            this.ButtonQueryClose.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.ButtonQueryClose_OnMouseUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ListViewTest = ((System.Windows.Controls.ListView)(target));
            
            #line 28 "..\..\QueryConstructorWindow.xaml"
            this.ListViewTest.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListViewTest_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DataGridColumns = ((System.Windows.Controls.DataGrid)(target));
            
            #line 31 "..\..\QueryConstructorWindow.xaml"
            this.DataGridColumns.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.DataGridColumns_OnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DataGridResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

