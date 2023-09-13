using FoodApp.MaterialDesign;
using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.Models
{
    public class ChoiceItem : BaseViewModel
    {
        public string Name { get; set; }

        public Choice<ChoiceItem> choice { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                OnPropertyChanged(nameof(TextDecoration));
                OnPropertyChanged(nameof(RadioIcon));
                OnPropertyChanged(nameof(CheckIcon));
            }
        }

        public TextDecorations TextDecoration
        {
            get => isSelected ? TextDecorations.Strikethrough : TextDecorations.None;
        }

        public string RadioIcon
        {
            get => isSelected ? Icons.RadioButtonChecked : Icons.RadioButtonUnchecked;
        }

        public string CheckIcon
        {
            get => isSelected ? Icons.CheckBox : Icons.CheckBoxOutlineBlank;
        }

        public ChoiceItem(string name)
        {
            Name = name;
        }
    }
}
