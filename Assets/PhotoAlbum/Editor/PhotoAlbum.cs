// Util > Generate PhotoAlbum
// 生成し終わったらMaterials/のBack以外のテクスチャに生成されたPhotoAlbum.assetを設定してください
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

public class PhotoAlbum : MonoBehaviour {

    [MenuItem ("Util/Generate PhotoAlbum")]
    static void Create () {
        // Resources/PhotoAlbum画像を配置名前はPhoto_0001のような1から始まるゼロ埋めの4桁
        // {0:000} Photo_0001, Photo_0002, Photo_0003, Photo_0004, ...
        // 事前に画像の設定でAdvanced > Read/Write Enableにチェックを入れておいてください
        string filePattern = "PhotoAlbum/Photo_{0:0000}";
        string dummy = "Dummy";

        // ファイルの数  ..., Photo_0242
        int slices = 242;

        // テクスチャとして使うので画像サイズは2の階乗 16:9画像なら1024:512なら縦の劣化はあまり気にならない
        Texture2DArray textureArray = new Texture2DArray (1024, 512, slices + 2, TextureFormat.RGB24, false);

        for (int i = 1; i <= slices; i++) {
            string filename = string.Format (filePattern, i);
            Texture2D tex = (Texture2D) Resources.Load (filename);
            textureArray.SetPixels (tex.GetPixels (0), i, 0);
        }
        for (int i = 0; i < 2; i++) {
            Texture2D dummyTex = (Texture2D) Resources.Load (dummy);
            textureArray.SetPixels (dummyTex.GetPixels (0), slices + 1, 0);
        }
        textureArray.Apply ();

        // テクスチャ配列をファイルに保存
        string arrayPath = "Assets/PhotoAlbum/PhotoAlbum.asset";
        AssetDatabase.CreateAsset (textureArray, arrayPath);
        Debug.Log ("Saved asset to " + arrayPath);

        // 自動生成するAnimationと AnimatorControllerを保存ディレクトリ
        string exportPath = "Assets/PhotoAlbum/Animation/";

        string[] animationPath = {
            "Big",
            "Smalls/Small",
            "Smalls/Small 1",
            "Smalls/Small 2",
            "Smalls/Small 3",
            "Smalls/Small 4",
            "Smalls/Small 5",
            "Smalls/Small 6",
            "Smalls/Small 7",
            "Smalls/Small 8",
            "Smalls/Small 9",
            "Smalls/Small 10",
            "Smalls/Small 11"
        };
        float keyframe = 1F / 30;

        // 自動生成するAnimatorControllerファイルの用意
        AssetDatabase.DeleteAsset (exportPath + "Page.controller");
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (exportPath + "Page.controller");
        var rootStateMachine = controller.layers[0].stateMachine;
        controller.AddParameter ("Int", AnimatorControllerParameterType.Int);
        controller.AddParameter ("Trigger", AnimatorControllerParameterType.Trigger);

        int pages = (slices - 1) / 12 + 1;

        for (int i = 0; i < pages; i++) {
            AnimationCurve curve = AnimationCurve.Linear (0, i, keyframe, i);
            AnimationClip page = new AnimationClip ();
            foreach (string path in animationPath) {
                page.SetCurve (path, typeof (MeshRenderer), "material._Page", curve);
            }
            // 自動生成するAnimationファイル名
            AssetDatabase.DeleteAsset (exportPath + "Pages/Page " + i.ToString () + ".anim");
            AssetDatabase.CreateAsset (page, exportPath + "Pages/Page " + i.ToString () + ".anim");
            Debug.Log ("Saved asset to " + exportPath+"Pages/Page "+i.ToString()+".anim");

            // 生成したAnimationファイルをControllerに登録
            var state = rootStateMachine.AddState ("state" + i.ToString ());
            state.motion = AssetDatabase.LoadAssetAtPath<AnimationClip> (exportPath + "Pages/Page " + i.ToString () + ".anim");

            var transition = rootStateMachine.AddAnyStateTransition (state);
            transition.AddCondition (UnityEditor.Animations.AnimatorConditionMode.Equals, i, "Int");
            transition.AddCondition (UnityEditor.Animations.AnimatorConditionMode.If, 0, "Trigger");
            transition.exitTime = 0;
            transition.hasFixedDuration = false;
            transition.duration = 0;
        }

    }
}
