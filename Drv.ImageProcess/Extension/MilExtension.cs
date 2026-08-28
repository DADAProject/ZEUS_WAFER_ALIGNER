using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drv.ImageProcess.Extension
{
    internal static class MilExtension
    {
        internal static byte[] GetBuffer_ByPoint(byte[] pBuf, int nPitch, int nW, int nH, int nX, int nY)
        {
        	if (!IsValidPtInImage(nW, nH, nX, nY))
        		return null; // pBuf;

            byte[] pbBuffer = new byte[nW];

			Buffer.BlockCopy(pBuf, (nY * nPitch) + nX, pbBuffer, 0, nW);
			//Array.Copy(pBuf, (nY * nPitch) + nX, pbBuffer, 0, nW);

            return pbBuffer;
        }
        internal static bool IsValidPtInImage(int nW, int nH, int nX, int nY)
        {
        	return !(nX < 0 || nY < 0);
        }

        internal static int pnpoly(int npol, PointF[] polygon, float x, float y)
        {
            int nConstant, nPolyNum, nTrueFalse_C = 0;
            for (nConstant = 0, nPolyNum = npol - 1; nConstant < npol; nPolyNum = nConstant++)
            {
                if (((y < polygon[nConstant].Y) != (y < polygon[nPolyNum].Y)) &&
                    (x < (polygon[nPolyNum].X - polygon[nConstant].X) * (y - polygon[nConstant].Y) / (polygon[nPolyNum].Y - polygon[nConstant].Y) + polygon[nConstant].X))
                    nTrueFalse_C++;
            }
            return nTrueFalse_C;
        }
		internal static void hswap(int[] rows, int[] columns, int i, int j)
		{
			int nSwitch_T;

			nSwitch_T = rows[i]; rows[i] = rows[j]; rows[j] = nSwitch_T;
			nSwitch_T = columns[i]; columns[i] = columns[j]; columns[j] = nSwitch_T;
		}
		internal static double angle_2pt(int r1, int c1, int r2, int c2)
		{
			double dAlgebraic_X, dWidth, dHeight;//, conv;

			//	conv = 180.0/PI;
			dWidth = (double)(r2 - r1);
			dHeight = (double)(c2 - c1);

			/*      Compute the raw angle based of Drow, Dcolumn            */
			if (dWidth == 0 && dHeight == 0)
			{
				dAlgebraic_X = 0.0;
			}
			else if (dHeight == 0)
			{
				dAlgebraic_X = 90.0;
			}
			else
			{
				dAlgebraic_X = Math.Abs(Math.Atan(dWidth / dHeight));
				dAlgebraic_X = dAlgebraic_X * 180.0 / Math.PI;
			}

			/*      Adjust the angle according to the quadrant              */
			if (dWidth <= 0)
			{                  /* upper 2 quadrants */
				if (dHeight < 0) dAlgebraic_X = 180.0 - dAlgebraic_X;    /* Left quadrant */
			}
			else if (dWidth > 0)
			{            /* Lower 2 quadrants */
				if (dHeight < 0)
				{
					dAlgebraic_X = dAlgebraic_X + 180.0;    /* Left quadrant */
				}
				else
				{
					dAlgebraic_X = 360.0 - dAlgebraic_X;             /* Right quadrant */
				}
			}

			return dAlgebraic_X;
		}


		internal static int GetConvexHullPt(int[] rows, int[] columns, int n)
		{
			int nCnt, nCntCpy, nMaxValueK;
			double dPrev, dBest, dAngleMin_X;

			/*  Find the pixel with the largest Row value               */
			nMaxValueK = 0;
			for (nCnt = 1; nCnt < n; nCnt++)
			{
				if (rows[nCnt] > rows[nMaxValueK])
					nMaxValueK = nCnt;
				else if ((rows[nCnt] == rows[nMaxValueK]) && (columns[nCnt] < columns[nMaxValueK]))
					nMaxValueK = nCnt;                  /* Same row, choose leftmost */
			}

			/*      Bottom-most point is row[k], column[k]. This will
			be the first point in the convex hull.                  */
			hswap(rows, columns, nMaxValueK, 0);
			rows[n] = rows[0]; columns[n] = columns[0];

			/*  The next point in the hull is always the point having the
			smallest angle measured from the previous point. The angles
			must increase as more pixels are added to the hull.             */
			dPrev = -1.0; nCntCpy = 0;
			int nMinValueK;
			do
			{
				dBest = 360.0; nMinValueK = -1;
				for (nCnt = nCntCpy + 1; nCnt <= n; nCnt++)
				{
					dAngleMin_X = angle_2pt(rows[nCntCpy], columns[nCntCpy], rows[nCnt], columns[nCnt]);

					if ((dAngleMin_X > dPrev) && (dAngleMin_X < dBest))
					{
						nMinValueK = nCnt; dBest = dAngleMin_X;
					}
					else if ((dAngleMin_X > dPrev) && (dAngleMin_X == dBest))
					{
						if ((Math.Abs(rows[nCnt] - rows[nCntCpy]) + Math.Abs(columns[nCnt] - columns[nCntCpy])) >
							(Math.Abs(rows[nMinValueK] - rows[nCntCpy]) + Math.Abs(columns[nMinValueK] - columns[nCntCpy])))
						{
							nMinValueK = nCnt; dBest = dAngleMin_X;
						}
					}
				}

				if (nMinValueK > 0)
				{
					dPrev = dBest;
					nCntCpy = nCntCpy + 1;
					hswap(rows, columns, nMinValueK, nCntCpy);
				}
			} while (nMinValueK > 0 && (nCntCpy < n));

			rows[nCntCpy + 1] = rows[0]; columns[nCntCpy + 1] = columns[0];
			return nCntCpy + 1;
		}

	}
}
