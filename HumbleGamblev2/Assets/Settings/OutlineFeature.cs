using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    class OutlinePass : ScriptableRenderPass
    {
        private Material outlineMaterial;
        private FilteringSettings filteringSettings;
        private ProfilingSampler profilingSampler = new ProfilingSampler("Outline Pass");

        public OutlinePass(Material material)
        {
            outlineMaterial = material;
            filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (outlineMaterial == null) return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, profilingSampler))
            {
                var drawSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, 
                    renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.overrideMaterial = outlineMaterial;
                drawSettings.overrideMaterialPassIndex = 1; // Usar el segundo pase (OutlinePass)

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private OutlinePass outlinePass;
    public Material outlineMaterial;

    public override void Create()
    {
        if (outlineMaterial == null)
        {
            outlineMaterial = CoreUtils.CreateEngineMaterial("Custom/OutlineOnly");
        }
        outlinePass = new OutlinePass(outlineMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (outlineMaterial != null)
        {
            renderer.EnqueuePass(outlinePass);
        }
    }
}