using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowards : MonoBehaviour
{
    [System.Serializable]
    public class IKWeight
    {
        public Transform bone;
        public float weight = 1f;
        public Quaternion lastlocal;
    }

    public Transform m_lookAtTarget;
    public List<IKWeight> m_HeadIkBones;
    float m_lookAtProgress = 0f;
    public float m_lookAtSpeed = 10f;
    public float m_lookAtSmooth = .3f;

    [Range(0.0f, 1.0f)]
    public float m_lookAt = 1f;


    private void Update()
    {
        LookAt();
    }

    //From Space Suit Girl package
    void LookAt()
    {
        if (m_lookAtTarget == null)
        {
            return;
        }

        for (int i = m_HeadIkBones.Count - 1; i >= 0; --i)
        {
            Quaternion r = m_HeadIkBones[i].bone.rotation;
            Vector3 forward = m_HeadIkBones[i].bone.parent.forward;

            if (Vector3.Dot(forward, (m_lookAtTarget.position - m_HeadIkBones[i].bone.position).normalized) > .4f)
            {
                m_lookAtProgress += Time.deltaTime * m_lookAtSpeed;
                m_lookAtProgress = Mathf.Clamp01(m_lookAtProgress);

                //look at
                Vector3 up = m_HeadIkBones[i].bone.parent.up;
                m_HeadIkBones[i].bone.LookAt(m_lookAtTarget, up);
                m_HeadIkBones[i].bone.rotation = Quaternion.Lerp(r, m_HeadIkBones[i].bone.rotation, m_HeadIkBones[i].weight * m_lookAt * m_lookAtProgress);


                m_HeadIkBones[i].bone.localRotation = Quaternion.Lerp(m_HeadIkBones[i].bone.localRotation, m_HeadIkBones[i].lastlocal, m_lookAtSmooth);


                //m_dontLookAtProgress = 0;
            }
            else
            {
                m_lookAtProgress -= Time.deltaTime * m_lookAtSpeed / 4f;
                m_lookAtProgress = Mathf.Clamp01(m_lookAtProgress);

                //m_lookAtProgress = 0;
                //m_dontLookAtProgress += Time.deltaTime;
                //m_dontLookAtProgress = Mathf.Clamp01(m_dontLookAtProgress);
                //don't look at
                m_HeadIkBones[i].bone.localRotation = Quaternion.Lerp(m_HeadIkBones[i].bone.localRotation, m_HeadIkBones[i].lastlocal, m_lookAtProgress);
                //m_HeadIkBones[i].lastlocal = m_HeadIkBones[i].bone.localRotation;
            }

            m_HeadIkBones[i].lastlocal = m_HeadIkBones[i].bone.localRotation;
        }
    }
}
