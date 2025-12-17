using UnityEngine;


public interface ICharcters
{
    void State();           //��ԊǗ����� 
    void Idle();            //�ҋ@���
    void Move();            //�ړ��A�s�����
    void End();              //�폜�@���@�A�j���[�V�����Ȃǂ̒ǉ��̍ۂɎg�p
    void SetMyState(int newState);
}



