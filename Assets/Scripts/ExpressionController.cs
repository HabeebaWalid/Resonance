using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ExpressionController : MonoBehaviour
{
    [Header ("SkinnedMeshRenderers")]
    public SkinnedMeshRenderer faceRenderer;
    public SkinnedMeshRenderer eyeOcclusionRenderer;

    private Dictionary <string, int> blendShapeIndices = new Dictionary <string, int>();
    private Dictionary <string, int> eyeOcclusionBlendShapeIndices = new Dictionary <string, int>();
    private Dictionary <int, float> targetWeights = new Dictionary <int, float>();
    private Dictionary <int, float> velocity = new Dictionary <int, float>();

    // Eye Blendshapes Apply To Eye Occlusion And Base Body Together
    private readonly HashSet <string> eyeBlendshapes = new HashSet <string>
    {
        "Eye_Wide_L", "Eye_Wide_R", "Eye_Squint_L", "Eye_Squint_R"
    };

    private readonly Dictionary <string, float> sadBlendshapes = new Dictionary <string, float>
    {
        { "Brow_Drop_L", 100f },
        { "Brow_Drop_R", 100f },
        { "Mouth_Frown_L", 100f },
        { "Mouth_Frown_R", 100f },
        { "Eye_Squint_L", 150f },
        { "Eye_Squint_R", 150f },
    };

    private readonly Dictionary <string, float> confusedBlendshapes = new Dictionary <string, float>
    {
        { "Brow_Raise_Inner_L", 20f },
        { "Brow_Raise_Inner_R", 20f },
        { "Brow_Drop_L", 40f },
        { "Brow_Drop_R", 40f },
        { "Eye_Wide_L", 20f },
        { "Eye_Wide_R", 20f },
    };

    private readonly Dictionary <string, float> amazedBlendshapes_Level1 = new Dictionary <string, float>
    {
        { "Brow_Raise_Inner_L", 60f },
        { "Brow_Raise_Inner_R", 60f },
        { "Eye_Wide_L", 60f },
        { "Eye_Wide_R", 60f },
        { "V_Tight_O", 32.5f },
        { "V_Tight", 35.9f },
        { "V_Wide", 100f },
        { "V_Lip_Open", 35.5f },
        { "Mouth_Frown_L", 25f },
        { "Mouth_Frown_R", 25f },
        { "Mouth_Shrug_Upper", 15.6f }
    };

    private readonly Dictionary <string, float> amazedBlendshapes_Level2 = new Dictionary <string, float>
    {
        { "Brow_Raise_Inner_L", 100f },
        { "Brow_Raise_Inner_R", 100f },
        { "Eye_Wide_L", 100f },
        { "Eye_Wide_R", 100f },
        { "V_Wide", 30f },
        { "Mouth_Shrug_Upper", 50f }
    };

    private readonly Dictionary <string, float> happyBlendshapes = new Dictionary <string, float>
    {
        { "Mouth_Smile_L", 60f },
        { "Mouth_Smile_R", 60f },
        { "Cheek_Raise_L", 80f },
        { "Cheek_Raise_R", 80f },
        { "Brow_Raise_Inner_L", 15f },
        { "Brow_Raise_Inner_R", 15f },
        { "V_Lip_Open", 17.5f },

    };

    private enum ExpressionState { None, Sad, Confused, Amazed, Happy }
    private ExpressionState currentExpression = ExpressionState.None;

    private float confusedBlendSpeed = 0.2f;
    private float amazedBlendSpeed = 1f;
    private float happyBlendSpeed = 0.2f;

    void Start()
    {
        if (faceRenderer == null)
        {
            Debug.LogError ("Assign Face SkinnedMeshRenderer");
            return;
        }

        CacheAllBlendShapeIndices();
        // Default Start Expression
        TriggerSad(); 
    }

    private void CacheAllBlendShapeIndices()
    {
        Mesh mesh = faceRenderer.sharedMesh;
        blendShapeIndices.Clear();
        eyeOcclusionBlendShapeIndices.Clear();

        string[] allShapes = new HashSet <string> (sadBlendshapes.Keys)
            .Union (confusedBlendshapes.Keys)
            .Union (amazedBlendshapes_Level1.Keys)
            .Union (amazedBlendshapes_Level2.Keys)
            .Union (happyBlendshapes.Keys)
            .ToArray();

        foreach (string name in allShapes)
        {
            // Face
            for (int i = 0; i < mesh.blendShapeCount; i++)
            {
                if (mesh.GetBlendShapeName(i).Contains(name))
                {
                    blendShapeIndices[name] = i;
                    velocity[i] = 0f;
                    break;
                }
            }

            // Eye Occlusion
            if (eyeBlendshapes.Contains(name) && eyeOcclusionRenderer != null)
            {
                Mesh eyeMesh = eyeOcclusionRenderer.sharedMesh;
                for (int i = 0; i < eyeMesh.blendShapeCount; i++)
                {
                    if (eyeMesh.GetBlendShapeName(i).Contains(name))
                    {
                        eyeOcclusionBlendShapeIndices[name] = i;
                        break;
                    }
                }

                if (!eyeOcclusionBlendShapeIndices.ContainsKey(name))
                    Debug.LogWarning($"Eye Blendshape '{name}' Not Found On EyeOcclusion Mesh.");
            }

            if (!blendShapeIndices.ContainsKey(name))
                Debug.LogWarning($"Blendshape '{name}' Not Found On Face Mesh.");
        }
    }
void Update()
{
    float speed = 0f;
    switch (currentExpression)
    {
        case ExpressionState.Confused:
            speed = confusedBlendSpeed;
            break;
        case ExpressionState.Amazed:
            speed = amazedBlendSpeed;
            break;
        case ExpressionState.Happy:
            speed = happyBlendSpeed;
            break;
    }

    var keys = targetWeights.Keys.ToList();
    foreach (var index in keys)
    {
        float target = targetWeights[index];
        float current = faceRenderer.GetBlendShapeWeight(index);

        if (speed > 0f)
        {
            float vel = velocity[index];
            float newWeight = Mathf.SmoothDamp (current, target, ref vel, 1f / speed);
            velocity[index] = vel;
            faceRenderer.SetBlendShapeWeight (index, newWeight);

            string shapeName = blendShapeIndices.FirstOrDefault (x => x.Value == index).Key;

            if (currentExpression != ExpressionState.Happy &&
                eyeOcclusionRenderer != null &&
                eyeBlendshapes.Contains(shapeName) &&
                eyeOcclusionBlendShapeIndices.TryGetValue (shapeName, out int eyeIndex))
            {
                eyeOcclusionRenderer.SetBlendShapeWeight (eyeIndex, newWeight);
            }
        }
        else
        {
            velocity[index] = 0f;
            faceRenderer.SetBlendShapeWeight (index, target);

            string shapeName = blendShapeIndices.FirstOrDefault (x => x.Value == index).Key;

           
            if (currentExpression != ExpressionState.Happy &&
                eyeOcclusionRenderer != null &&
                eyeBlendshapes.Contains(shapeName) &&
                eyeOcclusionBlendShapeIndices.TryGetValue (shapeName, out int eyeIndex))
            {
                eyeOcclusionRenderer.SetBlendShapeWeight (eyeIndex, target);
            }
        }
    }
}

    public void ResetAllBlendshapes()
    {
        foreach (var index in targetWeights.Keys.ToList())
        {
            targetWeights[index] = 0f;
        }

        currentExpression = ExpressionState.None;
    }

    public void TriggerSad()
    {
        SetTargetWeights (confusedBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level1.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level2.Keys.ToArray(), 0f);
        SetTargetWeights (happyBlendshapes.Keys.ToArray(), 0f);

        foreach (var pair in sadBlendshapes)
        {
            if (blendShapeIndices.TryGetValue (pair.Key, out int index))
            {
                targetWeights[index] = pair.Value;
                velocity[index] = 0f;
            }
        }

        currentExpression = ExpressionState.Sad;
    }

    public void TriggerConfused()
    {
        SetTargetWeights (sadBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level1.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level2.Keys.ToArray(), 0f);
        SetTargetWeights (happyBlendshapes.Keys.ToArray(), 0f);

        foreach (var pair in confusedBlendshapes)
        {
            if (blendShapeIndices.TryGetValue (pair.Key, out int index))
            {
                targetWeights[index] = pair.Value;
            }
        }

        currentExpression = ExpressionState.Confused;
    }

    public void TriggerAmazed (int level)
    {
        SetTargetWeights (sadBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (confusedBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level1.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level2.Keys.ToArray(), 0f);
        SetTargetWeights (happyBlendshapes.Keys.ToArray(), 0f);

        Dictionary <string, float> selected = level == 2 ? amazedBlendshapes_Level2 : amazedBlendshapes_Level1;

        foreach (var pair in selected)
        {
            if (blendShapeIndices.TryGetValue(pair.Key, out int index))
            {
                targetWeights[index] = pair.Value;
            }
        }

        currentExpression = ExpressionState.Amazed;
    }

    public void TriggerHappy()
    {
        SetTargetWeights (sadBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (confusedBlendshapes.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level1.Keys.ToArray(), 0f);
        SetTargetWeights (amazedBlendshapes_Level2.Keys.ToArray(), 0f);

        foreach (var pair in happyBlendshapes)
        {
            if (blendShapeIndices.TryGetValue (pair.Key, out int index))
            {
                targetWeights[index] = pair.Value;
                velocity[index] = 0f;
            }
        }

        currentExpression = ExpressionState.Happy;
    }

    private void SetTargetWeights (IEnumerable<string> shapeNames, float target)
    {
        foreach (string name in shapeNames)
        {
            if (blendShapeIndices.TryGetValue (name, out int index))
            {
                targetWeights[index] = target;
            }
        }
    }
}
