﻿#pragma checksum "..\..\..\TableauDebordUser.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7D65F486DFAB8B705C743EFE17FF05EEDAB6EB52"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using CannabisApp;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace CannabisApp {
    
    
    /// <summary>
    /// TableauDebordUser
    /// </summary>
    public partial class TableauDebordUser : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock UsernameTextBlock;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PlantesBonneSanteTextBlock;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PlantesNecessitantAttentionTextBlock;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path BackgroundArc;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path ProgressArc;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.PathFigure ProgressFigure;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ArcSegment ProgressSegment;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalPlantesText;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\TableauDebordUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PercentageText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CannabisApp;component/tableaudeborduser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\TableauDebordUser.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.UsernameTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            
            #line 30 "..\..\..\TableauDebordUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Deconnecter_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PlantesBonneSanteTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.PlantesNecessitantAttentionTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.BackgroundArc = ((System.Windows.Shapes.Path)(target));
            return;
            case 7:
            this.ProgressArc = ((System.Windows.Shapes.Path)(target));
            return;
            case 8:
            this.ProgressFigure = ((System.Windows.Media.PathFigure)(target));
            return;
            case 9:
            this.ProgressSegment = ((System.Windows.Media.ArcSegment)(target));
            return;
            case 10:
            this.TotalPlantesText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.PercentageText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            
            #line 94 "..\..\..\TableauDebordUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AjouterPlante_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 102 "..\..\..\TableauDebordUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AccederInventaire_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 110 "..\..\..\TableauDebordUser.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.VoirHistorique_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

