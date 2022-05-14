using System.Collections.Generic;

namespace Clover
{
    public class SequenceNode : WorkNode
    {
        readonly List<WorkNode> m_NodeList = new List<WorkNode>();
        int m_CurIndex;

        public override string Name => "Sequence";

        public SequenceNode()
        {
            m_CurIndex = -1;
        }

        public override bool isDone
        {
            get
            {
                if (m_CurIndex == -1)
                {
                    return false;
                }

                if (!Helper.IsValidIndex(m_CurIndex, m_NodeList.Count))
                {
                    return m_CurIndex >= m_NodeList.Count;
                }

                WorkNode node = m_NodeList[m_CurIndex];

                if (node != null && node.isDone)
                {
                    MoveToNextNode();
                }

                return m_CurIndex >= m_NodeList.Count;
            }
        }

        internal void Append(WorkNode node)
        {
            m_NodeList.Add(node);
        }

        protected override void OnStart()
        {
            m_CurIndex = -1;
            MoveToNextNode();
        }

        protected override void OnEnd()
        {
            for (int i = m_CurIndex; i >= 0 && i < m_NodeList.Count; ++i)
            {
                WorkNode node = m_NodeList[i];
                node?.End();
            }

            m_CurIndex = -1;

            base.OnEnd();
        }

        protected override void ResetToCache()
        {
            m_CurIndex = -1;
            m_NodeList.Clear();
            OnResetToCache();
        }

        public virtual void OnResetToCache() { }

        void MoveToNextNode()
        {
            if (Helper.IsValidIndex(m_CurIndex, m_NodeList.Count))
            {
                WorkNode node = m_NodeList[m_CurIndex];
                node.End();
            }

            ++m_CurIndex;

            if (Helper.IsValidIndex(m_CurIndex, m_NodeList.Count))
            {
                m_NodeList[m_CurIndex].Start();
            }
        }
    }
}