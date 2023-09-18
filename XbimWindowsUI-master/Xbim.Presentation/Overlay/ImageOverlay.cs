﻿using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xbim.Common.Geometry;

namespace Xbim.Presentation.Overlay
{
    public class ImageOverlay
    {
        internal BillboardVisual3D GraphicsItem;
        public enum ModelRelation
        {
            HidesBehindModel,
            AlwaysVisible
        }

        public XbimPoint3D Position { get; private set; }

        public double Width = 40;
        public double Height = 40;

		public ImageOverlay(
            string imagePath,
            XbimPoint3D position,
            ModelRelation modelRelation,
            double width = double.NaN,
            double height = double.NaN
            )
		{
            Position = position;
            Uri fileUri = new Uri(new Uri("file://"), imagePath);
            if (
                double.IsNaN(width) ||
                double.IsNaN(width))
            {
                var img = System.Drawing.Image.FromFile(imagePath);
                if (img != null)
                {
                    if (double.IsNaN(width))
                        width = img.Width;
                    if (double.IsNaN(height))
                        height = img.Height;
                }
            }

			GraphicsItem = new BillboardVisual3D()
			{
				Width = width,
				Height = height,
				Material = MaterialHelper.CreateEmissiveImageMaterial(
					fileUri.AbsoluteUri,
					Brushes.Black,
					UriKind.Absolute
					)
			};
            if (modelRelation == ModelRelation.AlwaysVisible)
            {
                GraphicsItem.DepthOffset = 0.1;
            }
			//GraphicsItem = new BillboardVisual3D()
			//{
            //             Width = width,
            //             Height = height,
            //             Material = MaterialHelper.CreateImageMaterial(
			//		fileUri.AbsoluteUri,
			//		1,
			//		UriKind.Absolute
			//		)
			//};

		}

        public void UpdatePosition(XbimModelPositioningCollection modelPositions, XbimPoint3D? newPosition = null)
        {
            if (newPosition.HasValue)
                Position = newPosition.Value;
            var pnt = modelPositions.GetPoint(Position);
            Point3D computedPoint = new Point3D(pnt.X, pnt.Y, pnt.Z);
            GraphicsItem.Position = computedPoint;
        }
    }
}
