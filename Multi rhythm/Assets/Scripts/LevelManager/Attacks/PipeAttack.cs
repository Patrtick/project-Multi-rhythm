using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Pipe")]
public class PipeAttack : AttackPattern
{
    [SerializeField] private string pipeRootName;
    [SerializeField] private float raiseSpeed = 2f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float warningTime = 2f;

    [Header("Настройки растения")]
    [SerializeField] private bool plantAttack = false;
    [SerializeField] private float plantRiseSpeed = 2f;
    [SerializeField] private float plantMaxHeight = 1.5f;
    [SerializeField] private float plantStayTime = 1.5f;

    [Header("Продолжительность, которая работает при отсутствии включённой атаки растения")]
    [SerializeField] private float duration = 10f;

    public override IEnumerator Execute(Transform origin)
    {
        var root = GameObject.Find(pipeRootName);
        if (root == null) yield break;

        var top = root.transform.Find("Pipe_Top");
        var body = root.transform.Find("Pipe_Bottom");
        var warning = root.transform.Find("Sign_!").gameObject;

        if (top == null || body == null || warning == null)
            yield break;

        Transform plant = null;
        var plantStartPos = Vector3.zero;

        if (plantAttack)
        {
            plant = top.Find("Plant");
            if (plant != null)
            {
                plant.gameObject.SetActive(false);
                plantStartPos = plant.localPosition;
            }
        }

        root.transform.position = origin.position;

        warning.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        warning.SetActive(false);

        var startTopPos = top.localPosition;
        var startBodyScaleY = body.localScale.y;

        var height = 0f;

        while (height < maxHeight)
        {
            height += raiseSpeed * Time.deltaTime;
            height = Mathf.Clamp(height, 0f, maxHeight);

            var offset = top.up * height;
            top.localPosition = startTopPos + offset;

            body.localScale = new Vector3(
                body.localScale.x,
                startBodyScaleY + height,
                body.localScale.z
            );

            yield return null;
        }

        if (!plantAttack)
            yield return new WaitForSeconds(duration);

        if (plantAttack && plant != null)
        {
            plant.gameObject.SetActive(true);

            var plantHeight = 0f;

            while (plantHeight < plantMaxHeight)
            {
                plantHeight += plantRiseSpeed * Time.deltaTime;

                plant.localPosition = plantStartPos + Vector3.up * plantHeight;
                yield return null;
            }

            yield return new WaitForSeconds(plantStayTime);

            while (plantHeight > 0f)
            {
                plantHeight -= plantRiseSpeed * Time.deltaTime;

                plant.localPosition = plantStartPos + Vector3.up * plantHeight;
                yield return null;
            }

            plant.localPosition = plantStartPos;
            plant.gameObject.SetActive(false);           
        }

        while (height > 0f)
        {
            height -= raiseSpeed * Time.deltaTime;

            var offset = top.up * height;
            top.localPosition = startTopPos + offset;

            body.localScale = new Vector3(
                body.localScale.x,
                startBodyScaleY + height,
                body.localScale.z
            );

            yield return null;
        }

        top.localPosition = startTopPos;
        body.localScale = new Vector3(body.localScale.x, startBodyScaleY, body.localScale.z);
    }
}