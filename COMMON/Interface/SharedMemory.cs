using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WF_Server
{
    public class TSharedMemory : IDisposable
    {
        //private static Mutex _mutex;
        //private static object _numLock;

        MemoryMappedFile            m_hMemoryMapped   = null;
        MemoryMappedViewAccessor    m_hMemoryAccessor = null;
        long m_nCapacity;

        //static TSharedMemory()
        //{
        //    _numLock = new object();
        //    if (!Mutex.TryOpenExisting("sharedMutex", out _mutex))
        //    {
        //        _mutex = new Mutex(true, "sharedMutex");
        //    }
        //}
        ~TSharedMemory() { Dispose(); }

        public void CreateShredMemory(string mapName, long capacity = 2048)
        {
            //lock (_numLock)
            //{
            //    if (_mutex.WaitOne())
            //    {
                    m_nCapacity = capacity;
                    m_hMemoryMapped     = MemoryMappedFile.CreateNew(mapName, capacity);
                    m_hMemoryAccessor   = m_hMemoryMapped.CreateViewAccessor(); 

            //        _mutex.ReleaseMutex();
            //    }
            //}
        }

        public void OpenSharedMemroy(string mapName, long capacity = 2048)
        {
            //lock (_numLock)
            //{
            //    if (_mutex.WaitOne())
            //    {
                    m_nCapacity = capacity;
                    m_hMemoryMapped     = MemoryMappedFile.OpenExisting(mapName);
                    m_hMemoryAccessor   = m_hMemoryMapped.CreateViewAccessor();

            //        _mutex.ReleaseMutex();
            //    }
            //}
        }

        public byte[] ReadMemory()
        {
            byte[] bytes = new byte[m_nCapacity];
            //
            //lock (_numLock)
            //{
            //    if (_mutex.WaitOne())
            //    {                    
                    m_hMemoryAccessor.ReadArray<byte>(0, bytes, 0, bytes.Length);
                    //
            //        _mutex.ReleaseMutex();
            //    }
            //}
            //
            return bytes;
        }

        public void WriteMemory(byte[] bytes)
        {
            //lock (_numLock)
            //{
            //    if (_mutex.WaitOne())
            //    {     
                    m_hMemoryAccessor.WriteArray<byte>(0, bytes, 0, bytes.Length);

            //        _mutex.ReleaseMutex();
            //    }
            //}
        }

        public void Dispose()
        {
            if (m_hMemoryAccessor != null)
            {
                m_hMemoryAccessor.Dispose();
                m_hMemoryAccessor = null;
            }
            if (m_hMemoryMapped != null)
            {
                m_hMemoryMapped.Dispose();
                m_hMemoryMapped = null;
            }
        }
    }
}

/* 사용예제
    //-------------------------------------------------
    //Server
    //-------------------------------------------------
    //객첵 생성.
    TSharedMemory[] sharedMem = new TSharedMemory[10];

    //Shared Memory Create.
    for (int n = 0; n < 10; n++)
    {
        sharedMemName = string.Format("SharedMem{0}", n);
        sharMem[n] = new TSharedMemory();
        sharMem[n].CreateShredMemory(sharedMemName);

    }    

    //Write.
    byte[] buffer = ASCIIEncoding.ASCII.GetBytes(textBox2.Text + "\0");
    if (sharMem[iIdx] == null) return;
    sharMem[iIdx].WriteMemory(buffer);


    //-------------------------------------------------
    //Client
    //-------------------------------------------------
    //객첵 생성.
    TSharedMemory[] sharedMem = new TSharedMemory[10];

    //Shared Memory Open.
    for (int n = 0; n < 10; n++)
    {
        sharedMemName = string.Format("SharedMem{0}", n);
        sharedMem[n] = new TSharedMemory();
        sharedMem[n].OpenSharedMemroy(sharedMemName);
    }

    //Read.
    byte[] buffer = new byte[2048];
    buffer = sharedMem[iIdx].ReadMemory();
    textBox2.Text = Encoding.Default.GetString(buffer);
 */
