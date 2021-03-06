﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using GraphicNotes.Views.Objects;
using GraphicNotes.Views.Workspace;
using System.Windows.Media;
using System.Windows.Controls;

namespace GraphicNotes.Views.Adorners
{
    public class MoveThumb : Thumb
    {
        private BaseObject element;
        private CanvasWorkspace canvasWorkspace;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.element = DataContext as BaseObject;

            if (this.element != null)
            {
                this.canvasWorkspace = VisualTreeHelper.GetParent(this.element) as CanvasWorkspace;
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.element != null && this.canvasWorkspace != null && this.element.State==BaseObject.ObjectState.Unlocked)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                foreach (BaseObject item in this.canvasWorkspace.SelectedElements)
                {
                    minLeft = Math.Min(Canvas.GetLeft(item), minLeft);
                    minTop = Math.Min(Canvas.GetTop(item), minTop);
                }

                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (BaseObject item in this.canvasWorkspace.SelectedElements)
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                    Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                }

                this.canvasWorkspace.InvalidateMeasure();
                e.Handled = true;
            }
        }

    }
}
