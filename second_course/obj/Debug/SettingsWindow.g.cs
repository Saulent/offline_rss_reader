﻿#pragma checksum "..\..\SettingsWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "81F0D5B73814F963C60B30A765F3BB791F214D98"
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
    /// SettingsWindow
    /// </summary>
    public partial class SettingsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 202 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle HeaderSettingsRectangle;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlockSettingsHeader;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ButtonSettingsClose;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ListBoxSources;
        
        #line default
        #line hidden
        
        
        #line 223 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonSaveFullDB;
        
        #line default
        #line hidden
        
        
        #line 224 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonLoadFullDB;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonResetNewspapers;
        
        #line default
        #line hidden
        
        
        #line 227 "..\..\SettingsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveExcelReport;
        
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
            System.Uri resourceLocater = new System.Uri("/second_course;component/settingswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SettingsWindow.xaml"
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
            this.HeaderSettingsRectangle = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 202 "..\..\SettingsWindow.xaml"
            this.HeaderSettingsRectangle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.HeaderSettingsRectangle_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBlockSettingsHeader = ((System.Windows.Controls.TextBlock)(target));
            
            #line 203 "..\..\SettingsWindow.xaml"
            this.TextBlockSettingsHeader.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.HeaderSettingsRectangle_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ButtonSettingsClose = ((System.Windows.Controls.Image)(target));
            
            #line 204 "..\..\SettingsWindow.xaml"
            this.ButtonSettingsClose.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.ButtonSettingsClose_OnMouseUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ListBoxSources = ((System.Windows.Controls.ListBox)(target));
            return;
            case 5:
            this.ButtonSaveFullDB = ((System.Windows.Controls.Button)(target));
            
            #line 223 "..\..\SettingsWindow.xaml"
            this.ButtonSaveFullDB.Click += new System.Windows.RoutedEventHandler(this.ButtonSaveFullDB_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ButtonLoadFullDB = ((System.Windows.Controls.Button)(target));
            
            #line 224 "..\..\SettingsWindow.xaml"
            this.ButtonLoadFullDB.Click += new System.Windows.RoutedEventHandler(this.ButtonLoadFullDB_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 225 "..\..\SettingsWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ButtonResetNewspapers = ((System.Windows.Controls.Button)(target));
            
            #line 226 "..\..\SettingsWindow.xaml"
            this.ButtonResetNewspapers.Click += new System.Windows.RoutedEventHandler(this.ButtonResetNewspapers_OnClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.SaveExcelReport = ((System.Windows.Controls.Button)(target));
            
            #line 227 "..\..\SettingsWindow.xaml"
            this.SaveExcelReport.Click += new System.Windows.RoutedEventHandler(this.SaveExcelReport_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
