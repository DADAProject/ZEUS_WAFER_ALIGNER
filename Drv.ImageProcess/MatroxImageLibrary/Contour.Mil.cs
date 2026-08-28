using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Matrox.MatroxImagingLibrary;

namespace Drv.ImageProcess
{
	internal partial class Contour
	{
		internal void ContourTrace(MIL_ID mSrc, ref ContourPoints cp)
		{
			BUFF biProc1 = new BUFF();
			biProc1.buffID = (long)mSrc;
			biProc1.InitBuffInfo(false, 0);

			int[][] nDir = new int[8][];   // 진행 방향을 나타내는 배열
			nDir[0] = new int[2] { 1, 0 };
			nDir[1] = new int[2] { 1, 1 };
			nDir[2] = new int[2] { 0, 1 };
			nDir[3] = new int[2] { -1, 1 };
			nDir[4] = new int[2] { -1, 0 };
			nDir[5] = new int[2] { -1, -1 };
			nDir[6] = new int[2] { 0, -1 };
			nDir[7] = new int[2] { 1, -1 };

			cp = new ContourPoints();
			cp.num = 0;

			int nSkip = 1;
			int nDOld, nD, nCnt;
			int nx, ny;
			for (int nIy = 0; nIy < biProc1.len; nIy += nSkip)
			{
				for (int nIx = 0; nIx < biProc1.wid; nIx += nSkip)
				{
					if (biProc1.pBuff[nIy * biProc1.pitch + nIx] != 255)
					{
						int nCpyX = nIx;
						int nCpyY = nIy;

						nDOld = nD = nCnt = 0;
						while (true)
						{
							nx = nCpyX + nDir[nD][0];
							ny = nCpyY + nDir[nD][1];

							if (nx < 0 || nx >= biProc1.wid || ny < 0 || ny >= biProc1.len || biProc1.pBuff[ny * biProc1.pitch + nx] == 255)
							{
								// 진행 방향에 있는 픽셀이 객체가 아닌 경우,
								// 시계 방향으로 진행 방향을 바꾸고 다시 시도한다.

								if (++nD > 7) nD = 0;
								nCnt++;

								// 8방향 모두 배경(Background_255)인 경우
								if (nCnt >= 8)
								{
									cp.Points.Add(new Point2f(nCpyX, nCpyY));
									cp.num++;
									break;
								}
							}
							else
							{
								// 진행방향의 픽셀이 객체일 경우
								// 현재 점을 외곽선 정보에 저장

								cp.Points.Add(new Point2f(nCpyX, nCpyY));
								cp.num++;

								if (cp.num >= MAX_CONTOUR)
									break;  // 외곽선 픽셀이 너무 많으면 강제 종료

								// 진행방향으로 이동
								nCpyX = nx;
								nCpyY = ny;

								// 방향 정보 초기화
								nCnt = 0;
								nDOld = nD;
								nD = (nDOld + 6) % 8;       // d = dOld - 2와 같음
							}

							// 시작점으로 돌아왔고, 진행 방향이 초기화된 경우
							// 외곽선 추적을 끝낸다.
							if (nCpyX == nIx && nCpyY == nIy && nD == 0)
								break;
						}

						// for 루프를 강제로 종료하기 위해 i, j값을 조정한다.
						nIx = biProc1.wid;
						nIy = biProc1.len;
					}
				}
			}
		}
	}
}
