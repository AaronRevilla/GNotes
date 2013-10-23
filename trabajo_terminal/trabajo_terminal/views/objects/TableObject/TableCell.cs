using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class TableCell:TextEdit
    {
        

        private CellBorder[] borders;

        public TableCell(){
            borders = new CellBorder[4];
        }

        public CellBorder GetCellBorder(int index ){
            return borders[index];
        }

        public void SetCellBorder(CellBorder border, int index)
        {
            borders[index]=border;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected", typeof(bool),
                                      typeof(TableCell),
                                      new FrameworkPropertyMetadata(false));
    }
}
