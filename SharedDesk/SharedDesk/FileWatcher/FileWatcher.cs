using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDesk.FileWatcher
{
    class FileWatcher
    {
        private StringBuilder m_Sb;
        private bool m_bMyBool;
        private FileSystemWatcher m_Watcher;
        private bool m_bIsWatching;
        private bool m_bIsChecked;

        public bool _m_bMyBool
        {
            //get m_bMyBool
            get{return this.m_bMyBool;}
            //set m_bMyBool
            set { this.m_bMyBool = value; }
        }

        public FileWatcher (bool isChecked)
        {
            m_bIsChecked = isChecked;
            m_Sb = new StringBuilder();
        }


        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!m_bMyBool)
            {
                m_Sb.Remove(0, m_Sb.Length);
                m_Sb.Append(e.FullPath);
                m_Sb.Append(" ");
                m_Sb.Append(e.ChangeType.ToString());
                m_Sb.Append("    ");
                m_Sb.Append(DateTime.Now.ToString());
                m_bMyBool = true;
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (!m_bMyBool)
            {
                m_Sb.Remove(0, m_Sb.Length);
                m_Sb.Append(e.OldFullPath);
                m_Sb.Append(" ");
                m_Sb.Append(e.ChangeType.ToString());
                m_Sb.Append(" ");
                m_Sb.Append("to ");
                m_Sb.Append(e.Name);
                m_Sb.Append("    ");
                m_Sb.Append(DateTime.Now.ToString());
                m_bMyBool = true;
                if (m_bIsChecked)
                {
                    m_Watcher.Filter = e.Name;
                    m_Watcher.Path = e.FullPath.Substring(0, e.FullPath.Length - m_Watcher.Filter.Length);
                }
            }
        }

        public void WatchFile(string path)
        {
            m_bIsWatching = true;

            m_Watcher = new System.IO.FileSystemWatcher();
            m_Watcher.Filter = "*.*";
            m_Watcher.Path = path + "\\";
            
            if (m_bIsChecked)
            {
                m_Watcher.IncludeSubdirectories = true;
            }

            m_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
            m_Watcher.Created += new FileSystemEventHandler(OnChanged);
            m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
            m_Watcher.Renamed += new RenamedEventHandler(OnRenamed);
            m_Watcher.EnableRaisingEvents = true;
        }

        public string sbToString()
        {
            string tmp = m_Sb.ToString();
            return tmp;
        }
    }
}
