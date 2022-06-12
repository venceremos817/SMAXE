using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChunk : MonoBehaviour
{
    [ReadOnly]
    public int cellID = 0;

    public enum SHIFT
    {
        SHIFT_UP,
        SHIFT_DOWN,
        SHIFT_LEFT,
        SHIFT_RIGHT
    };

    /// <summary>
    /// �X�e�[�W�N���[��
    /// </summary>
    /// <param name="shift">�X�N���[������</param>
    /// <param name="chunkSize">1�`�����N�̑傫��</param>
    /// <param name="grid">�X�e�[�W��grid*grid�̃`�����N������</param>
    /// <param name="followLayer">�X�N���[���ɂ��Ă���I�u�W�F�N�g�̃��C���[</param>
    public void Shift(SHIFT shift, Vector2 chunkSize, byte grid, LayerMask followLayer)
    {
        var stageGenerator = StageGenerator.Instance;
        Vector3 shiftV = new Vector3();

        switch (shift)
        {
            // ��փV�t�g
            case SHIFT.SHIFT_UP:
                if ((cellID - grid) < 0)
                {
                    shiftV.Set(0.0f, chunkSize.y * grid, 0.0f);
                    var objects = GetRandingObjects(chunkSize);
                    foreach (var obj in objects)
                        if (((1 << obj.gameObject.layer) & followLayer.value) != 0)
                            obj.transform.position += shiftV;
                    transform.position += shiftV;
                    cellID += grid * (grid - 1);
                }
                else
                {
                    cellID -= grid;
                }
                break;

            // ���փV�t�g
            case SHIFT.SHIFT_DOWN:
                if ((cellID - grid * (grid - 1)) >= 0)
                {
                    shiftV.Set(0.0f, -chunkSize.y * grid, 0.0f);
                    var objects = GetRandingObjects(chunkSize);
                    foreach (var obj in objects)
                        if (((1 << obj.gameObject.layer) & followLayer.value) != 0)
                            obj.transform.position += shiftV;
                    transform.position += shiftV;
                    cellID -= grid * (grid - 1);
                }
                else
                {
                    cellID += grid;
                }
                break;

            // ���փV�t�g
            case SHIFT.SHIFT_LEFT:
                if ((cellID - grid + 1) % grid == 0)
                {
                    shiftV.Set(-chunkSize.x * grid, 0.0f, 0.0f);
                    var objects = GetRandingObjects(chunkSize);
                    foreach (var obj in objects)
                        if (((1 << obj.gameObject.layer) & followLayer.value) != 0)
                            obj.transform.position += shiftV;
                    transform.position += shiftV;
                    cellID -= grid - 1;
                }
                else
                {
                    cellID += 1;
                }
                break;

            // �E�փV�t�g
            case SHIFT.SHIFT_RIGHT:
                if ((cellID % grid) == 0)
                {
                    shiftV.Set(chunkSize.x * grid, 0.0f, 0.0f);
                    var objects = GetRandingObjects(chunkSize);
                    foreach (var obj in objects)
                        if (((1 << obj.gameObject.layer) & followLayer.value) != 0)
                            obj.transform.position += shiftV;
                    transform.position += shiftV;
                    cellID += grid - 1;
                }
                else
                {
                    cellID -= 1;
                }
                break;
        }

        // �����`�����N�m��
        if (cellID == grid * grid / 2)
            StageGenerator.Instance.centerChunkPos = transform.position;
    }

    /// <summary>
    /// �`�����N�ɏ���Ă���I�u�W�F�N�g���擾
    /// </summary>
    /// <param name="chunkSize"></param>
    /// <returns></returns>
    private Collider2D[] GetRandingObjects(Vector2 chunkSize)
    {
        Vector2 center = transform.position;
        chunkSize.x *= -1.0f;
        Vector2 pointA = center + chunkSize * 0.5f;
        Vector2 pointB = center - chunkSize * 0.5f;
        var objects = Physics2D.OverlapAreaAll(pointA, pointB);

        return objects;
    }
}