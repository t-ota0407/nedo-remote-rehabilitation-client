using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingQuality : MonoBehaviour
{
    private void Awake()
    {
        // ��ʉ𑜓x scale 1.0 == 1024x1024 �` 1.25 == 1280x1280(�����I�ȉ𑜓x�̌��E) 0.5f�Ȃǂ��t���[�����[�g���҂��̂ɗL���B
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 1.25f;

        // �O�g���̃����_�����O���r�����邱�Ƃő��x���҂��B Off/LMSLow/LMSMedium/LMSHigh�@�O���̉𑜓x�������Ă���
        // OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;

        // 72Hz���[�h�i�Y��ɂȂ邪�t���[�����[�g���肪�]���ɂȂ�j
        //OVRManager.display.displayFrequency = 72.0f;

        // CPU,GPU�̃p�t�H�[�}���X���x���B0�`3�̑傫���قǕ��M���������X�y�b�N�ȓ��������B����l��2�B
        //OVRManager.cpuLevel = 2;
        //OVRManager.gpuLevel = 2;

        // �e�N�X�`���𑜓x �f�t�H���g��1.0 �グ��Ώグ��ق�VR���̃W���M�[�������ĂȂ߂炩�ɕ`��ł���B(���ʂ͐�傾�����I�ɏd���Ȃ�)
        //UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 2.0f;

        // ��ʒ[�̕����Ԃ��ۂ��Ȃ邱�Ƃւ̕␳������ (adb shell setprop debug.oculus.forceChroma 1 ����)
        //OVRPlugin.chromatic = true;

        // �A���`�G�C���A�V���O�B���܂�����ł��Ȃ�
        // QualitySettings.antiAliasing = 4;

        // �V���O���p�X�X�e���I�����_�����O (���ʂ͉E�ڗp�ƍ��ڗp��2�񃌃��_�����O���s���Ƃ�����A1�V�[���ɂ���𖳗���艟�����߂��@)
        // Edit > ProjectSettings > Player
        //   XRSettings > StereoRenderingMethod: MultiPass��SinglePass

        // �EOVRCameraRig��OVR Manager�ɂ���Enable Adaptive Resolution���I�t�ɂ���
        // GPU�̎g�p���������Ȃ�ƃt���[���������Ȃ��悤�Ɏ����I�ɉ𑜓x��������@�\
        // ���Ȃ���ʂ��������Ƃ̂���
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
