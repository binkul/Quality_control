﻿using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.AddNew.Model;
using Quality_Control.Forms.Modification.Model;
using Quality_Control.Forms.Setting.Command;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Setting.ModelView
{
    internal class SettingMV : INotifyPropertyChanged
    {
        private ICommand _saveButton;
        private ICommand _deleteButton;
        private ICommand _setButton;
        private ICommand _stdButton;
        private ICommand _stdpHButton;
        private ICommand _copyButton;

        private readonly double _startLeftPosition = 32;
        private readonly ProductService _service = new ProductService();
        private readonly ModificationService _serviceMod = new ModificationService();
        private int _selectedIndex;
        public RelayCommand<TextChangedEventArgs> OnProductNameFilterTextChanged { get; set; }
        public RelayCommand<TextChangedEventArgs> OnProductNumberFilterTextChanged { get; set; }

        public SettingMV()
        {
            OnProductNameFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductNameTextChangedFilter);
            OnProductNumberFilterTextChanged = new RelayCommand<TextChangedEventArgs>(OnProductIndexTextChangedFilter);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<ProductModel> Products => _service.FilteredProducts;

        public bool IsAnyProduct => Products.Count > 0;

        public List<ModificationModel> Fields => _serviceMod.Fields;

        public Thickness TxtIndexLeftPosition => new Thickness(_startLeftPosition, 0, 0, 5);

        internal bool IsAnySet => _serviceMod.IsAnySet;

        internal bool IsAnyUnset => _serviceMod.IsAnyUnSet;

        internal bool Modified => _serviceMod.Modified || _service.Modified;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_serviceMod.Modified)
                {
                    Products[_selectedIndex].ActiveFields = _serviceMod.RecalculateFields();
                }
                _selectedIndex = value;
                _serviceMod.UnsetFields();
                if (IsAnyProduct && !string.IsNullOrEmpty(Products[_selectedIndex].ActiveFields))
                    _serviceMod.CheckFieldsInList(Products[_selectedIndex].ActiveFields);
                _serviceMod.UnModifiedAll();
                OnPropertyChanged(nameof(IsAnyProduct));
            }
        }

        public ICommand SaveButton
        {
            get
            {
                if (_saveButton == null) _saveButton = new SaveFieldsButton(this);
                return _saveButton;
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                if (_deleteButton == null) _deleteButton = new DeleteButton(this);
                return _deleteButton;
            }
        }

        public ICommand SetButton
        {
            get
            {
                if (_setButton == null) _setButton = new SetButton(this);
                return _setButton;
            }
        }

        public ICommand StdButton
        {
            get
            {
                if (_stdButton == null) _stdButton = new StandardButton(this);
                return _stdButton;
            }
        }

        public ICommand StdWithpHButton
        {
            get
            {
                if (_stdpHButton == null) _stdpHButton = new StandardpHButton(this);
                return _stdpHButton;
            }
        }

        public ICommand CopyButton
        {
            get
            {
                if (_copyButton == null) _copyButton = new CopyButton(this);
                return _copyButton;
            }
        }

        internal void Save()
        {
            if (_serviceMod.Modified)
            {
                Products[SelectedIndex].ActiveFields = _serviceMod.RecalculateFields();
                _serviceMod.UnModifiedAll();
            }

            _service.Save();
        }

        internal void Copy()
        {
            _serviceMod.Copy();
        }

        internal void Delete()
        {
            _serviceMod.UnsetFields();
        }

        internal void Set()
        {
            _serviceMod.SetFields();
        }

        internal void Standard()
        {
            _serviceMod.UnsetFields();
            _serviceMod.SetStandard();
        }

        internal void StandardpH()
        {
            _serviceMod.UnsetFields();
            _serviceMod.SetStandardWithpH();
        }

        #region Filtering

        public string FilterNameTxt { get; set; } = "";

        public string NumberFilterTxt { get; set; } = "";

        public void OnProductNameTextChangedFilter(TextChangedEventArgs e)
        {
            int number = !string.IsNullOrEmpty(NumberFilterTxt) ? Convert.ToInt32(NumberFilterTxt) : -1;
            _service.Filter(number, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
            SelectedIndex = 0;
        }

        public void OnProductIndexTextChangedFilter(TextChangedEventArgs e)
        {
            int number = !string.IsNullOrEmpty(NumberFilterTxt) ? Convert.ToInt32(NumberFilterTxt) : -1;
            _service.Filter(number, FilterNameTxt);
            OnPropertyChanged(nameof(Products));
            SelectedIndex = 0;
        }

        #endregion

    }
}
