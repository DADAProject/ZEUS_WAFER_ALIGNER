using System.IO;

namespace eMachine
{
    public partial class TAlgorithm
    {
        public bool CheckRegion(Drv.ImageProcess.Rect region)
        {
            if (region.Width == 0 && region.Height == 0    ) return false;

            if (region.X      < 0) return false;
            if (region.Y      < 0) return false;
            if (region.Width  <= 0) return false;
            if (region.Height <= 0) return false;

            return true;
        }

        public Drv.ImageProcess.Rect GetRegion(string name)
        {
            TROI cRoi = TROI.GeRegion(name);

            return new Drv.ImageProcess.Rect((int)cRoi.dX, (int)cRoi.dY, (int)cRoi.dWidth, (int)cRoi.dHeight);
        }

        public bool CheckReference(string name)
        {
            return File.Exists(name);
        }

        public TReference GetReference(string name)
        {
            TReference cRef = TReference.GetReference(name);

            return cRef;
        }


        public Drv.ImageProcess.BUFF GetReferenceBUFF(string name)
        {
            TReference cRef = TReference.GetReference(name);

            Drv.ImageProcess.BUFF tRef = new Drv.ImageProcess.BUFF(Drv.ImageProcess.BufferType.VisionPro);
            tRef.ImportBuffInfo(name);

            return tRef;
        }

        public bool CheckReference(Drv.ImageProcess.BUFF reference)
        {
            if (reference.Allocated == false) return false;
            if (reference.wid       <= 0    ) return false;
            if (reference.len       <= 0    ) return false;

            return true;
        }


        
    }
}
