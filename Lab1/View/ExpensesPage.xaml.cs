using Lab1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1.Views;
public partial class ExpensesPage : ContentPage
{
    public ExpensesPage()
    {
        InitializeComponent();

        BindingContext = new ExpensesViewModel();
    }
}