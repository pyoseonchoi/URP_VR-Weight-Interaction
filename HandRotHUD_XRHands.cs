using TMPro;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.Hands;

public class HandRotHUD_XRHands : MonoBehaviour
{
    public TextMeshProUGUI text;
    XRHandSubsystem hands;

    void OnEnable()
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader != null)
            hands = loader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    void LateUpdate()
    {
        if (!text) return;

        if (hands == null || !hands.running)
        {
            text.text = "XRHands not running";
            return;
        }

        string Line(XRHand hand, string label)
        {
            if (!hand.isTracked) return $"{label}: not tracked\n";

            // 손목 포즈
            var wrist = hand.GetJoint(XRHandJointID.Wrist);
            Pose wp;
            if (!wrist.TryGetPose(out wp)) return $"{label}: wrist pose N/A\n";

            // 손바닥도 가능
            var palm = hand.GetJoint(XRHandJointID.Palm);
            Pose pp;
            palm.TryGetPose(out pp);

            Vector3 pr = wp.rotation.eulerAngles;
            return $"{label} Wrist  P({wp.position.x:0.00},{wp.position.y:0.00},{wp.position.z:0.00})  " +
                   $"R({pr.x:0},{pr.y:0},{pr.z:0})\n";
        }

        text.text = Line(hands.leftHand, "Left") +
                    Line(hands.rightHand, "Right");
    }
}

