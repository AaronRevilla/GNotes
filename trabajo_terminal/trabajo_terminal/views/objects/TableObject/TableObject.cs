using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicNotes.Views.Objects
{
    class TableObject:BaseObject
    {
        Grid gridTable;
        private int rows;
        private int columns;

        static TableObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TableObject), new FrameworkPropertyMetadata(typeof(TableObject)));
            
        }


        public TableObject(int columns, int rows)
        {
            // TODO: Complete member initialization
            this.columns = columns;
            this.rows = rows;
            this.Loaded += new RoutedEventHandler(this.TableObject_Loaded);
            
        }

        private void TableObject_Loaded(object sender, RoutedEventArgs e)
        {
            if(gridTable==null)
                gridTable=this.Template.FindName("PART_Grid", this) as Grid;
            CreateTable(columns, rows);
        }

        public IEnumerable<TableCell> SelectedElements
        {
            get
            {
                var selectedElements = from element in gridTable.Children.OfType<TableCell>()
                                       where element.IsSelected == true
                                       select element;

                return selectedElements;
            }
        }

        public void CreateTable(int columns, int rows){

            gridTable = this.Template.FindName("PART_Grid", this) as Grid;
            if (gridTable != null)
            {

                gridTable.Children.Clear();
                int realRows = (rows * 2) + 1;
                int realColumns=(columns*2)+1;

                for (int i = 0; i < realColumns; i++)
                {
                    AddColumnDefinition(gridTable.ColumnDefinitions.Count);
                    if (i % 2 == 0)
                        AddColumnGridSplitter(i);

                    for (int j = 0; j < realRows; j++)
                    {
                        if(i==0)
                        AddRowDefinition(gridTable.RowDefinitions.Count);
                        if (j % 2 == 0)
                            AddRowGridSplitter(j);

                        if (i % 2 != 0 && j % 2 != 0)
                        {
                           AddTableCell(i, j);
                        }
                    }
                }
                
            }
        }

        private void AddColumnDefinition(int index)
        {
            ColumnDefinition cd = new ColumnDefinition();
            if (index % 2 == 0)
                cd.Width = GridLength.Auto;
            else
                cd.Width = new GridLength(1, GridUnitType.Star);
                
            gridTable.ColumnDefinitions.Add(cd);
        }

        private void AddRowDefinition(int index)
        {
            RowDefinition rd = new RowDefinition();
            if (index % 2 == 0)
                rd.Height = GridLength.Auto;
            else
                rd.Height = new GridLength(1, GridUnitType.Star);

            gridTable.RowDefinitions.Add(rd);
        }

        private void AddColumnGridSplitter(int index){
            GridSplitter gs = new GridSplitter();
            gs.SetResourceReference(GridSplitter.StyleProperty, "GridSplitter_Column");
            Grid.SetColumn(gs, index);
            gridTable.Children.Add(gs);
        }

        private void AddRowGridSplitter(int index)
        {
            GridSplitter gs = new GridSplitter();
            gs.SetResourceReference(GridSplitter.StyleProperty, "GridSplitter_Row");
            Grid.SetRow(gs, index);
            gridTable.Children.Add(gs);
        }

        private void AddTableCell(int column, int row)
        {
            TableCell tc = new TableCell();
            tc.SetResourceReference(TableCell.StyleProperty, "TableObject_CellStyle");
            Grid.SetColumn(tc, column);
            Grid.SetRow(tc, row);
            gridTable.Children.Add(tc);
            SetBorders(tc);
            for (int i = 0; i < 4; i++)
            {
                if (!gridTable.Children.Contains(tc.GetCellBorder(i)))
                    gridTable.Children.Add(tc.GetCellBorder(i));
            }
        }

        private void SetBorders(TableCell cell)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = -2; j < 3; j += 4)
                {
                    IEnumerable<TableCell> query;
                    if(i==0)
                        query = (from element in gridTable.Children.OfType<TableCell>()
                                     where Grid.GetColumn(element) == Grid.GetColumn(cell)
                                     && Grid.GetRow(element) == Grid.GetRow(cell)+j
                                     select element);
                    else
                        query = (from element in gridTable.Children.OfType<TableCell>()
                                 where Grid.GetColumn(element) == Grid.GetColumn(cell)+j
                                 && Grid.GetRow(element) == Grid.GetRow(cell)
                                 select element);

                    if (query.Count() > 0)
                    {
                        
                        if (i == 0)
                        {
                            //Top Border
                            if (j == -2)
                                cell.SetCellBorder(query.ToArray()[0].GetCellBorder(2), 0);
                            //Bottom Border
                            if (j == 2)
                                cell.SetCellBorder(query.ToArray()[0].GetCellBorder(0), 2);
                        }
                        else
                        {
                            //Left Border
                            if (j == -2)
                                cell.SetCellBorder(query.ToArray()[0].GetCellBorder(1),3);
                            //Right Border
                            if (j == 2)
                                cell.SetCellBorder(query.ToArray()[0].GetCellBorder(3), 1);
                        }
                    }
                    else
                    {
                        CellBorder cb = new CellBorder();
                        if (i == 0)
                        {
                            cb.SetResourceReference(CellBorder.StyleProperty, "CellBorder_Horizontal");
                            Grid.SetColumn(cb,Grid.GetColumn(cell)-1);
                            
                            //Top Border
                            if (j == -2)
                            {
                                Grid.SetRow(cb, Grid.GetRow(cell) - 1);
                                cell.SetCellBorder(cb, 0);
                            }
                            //Bottom Border
                            if (j == 2)
                            {
                                Grid.SetRow(cb, Grid.GetRow(cell) + 1);
                                cell.SetCellBorder(cb, 2);
                            }
                        }
                        else
                        {
                            cb.SetResourceReference(CellBorder.StyleProperty, "CellBorder_Vertical");
                            Grid.SetRow(cb, Grid.GetRow(cell)-1);
                            
                            //Left Border
                            if (j == -2)
                            {
                                Grid.SetColumn(cb, Grid.GetColumn(cell) - 1);
                                cell.SetCellBorder(cb, 3);
                            }
                            //Right Border
                            if (j == 2)
                            {
                                Grid.SetColumn(cb, Grid.GetColumn(cell) + 1);
                                cell.SetCellBorder(cb, 1);
                            }
                        }
                    }
                }
            }
                
        }
    }
}
