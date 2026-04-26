using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Pipe")]
public class PipeAttack : AttackPattern
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private string pipeRootName;
    [SerializeField] private float raiseSpeed = 2f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float warningTime = 2f;

    public override IEnumerator Execute(Transform origin)
    {
        var root = GameObject.Find(pipeRootName);
        if (root == null) yield break;

        var top = root.transform.Find("Pipe_Top");
        var body = root.transform.Find("Pipe_Bottom");
        var warning = root.transform.Find("Sign_!").gameObject;

        if (top == null || body == null || warning == null)
            yield break;

        root.transform.position = origin.position;

        warning.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        warning.SetActive(false);

        var startTopPos = top.localPosition;
        var startBodyScaleY = body.localScale.y;

        var timer = 0f;
        var height = 0f;

        while (timer < duration)
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

            timer += Time.deltaTime;
            yield return null;
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