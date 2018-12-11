# VRCPhotoAlbum
## 自分のHomeワールドで使っている大量の画像をいい感じに管理するやつです
- そのうちこのあたりにいい感じのデモ動画が差し込まれるはず
### 事前準備
1. 適当なオブジェクトを選択して`Inspector > Layer > Add Layer...`から`User Layer 31`に`PhotoSystem`を追加する
1. 入れたい画像を1024x512にリサイズして`Photo_0001.png`,`Photo_0002.png`のようにリネームする
    - `php,imagick`の動く環境があれば`Script/conv.php`で変換可
### サンプルを動かす
1. VRCSDKとVRChat_HomeKitをインポートする
1. PhotoAlbum.unitypackageをインポートする
1. 用意した画像を`Resources`フォルダに入れて`Inspector > Advanced > Read/Write Enabled`にチェックを入れる
1. `Assets/PhotoAlbum/Editor/PhotoAlbum.cs`の19行目の`int slices = 242;`の242を入れる画像の枚数に設定する (例:Photo_0001.png~Photo_0100.pngまでを入れるなら101)
1. `Assets/PhotoAlbum/PhotoAlbum.shader`を開き6行目の`_SliceRange("Slices", Range(0,243)) = 0`の243を入れる画像の枚数+1に設定する (例:Photo_0001.png~Photo_0100.pngまでを入れるなら101)
1. ツールバーから`Util > Generate PhotoAlbum`を実行する
1. 生成された`Assets/PhotoAlbum/PhotoAlbum.asset`を`Assets/PhotoAlbum/Materials`以下の`Back以外`のマテリアルのTexに指定する
1. `Assets/PhotoAlbum/PhotoSample.unity`を開く
