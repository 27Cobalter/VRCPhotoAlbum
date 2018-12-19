// フォルダ構成
// |- conv.php
// |- pic/
// |  |- 変換前1.png
// |  `- 変換前2.png
// `- resized/
<?php
$project='./';
$files=scandir($project.'pic', 1);
foreach($files as $num => $file){
  // 元画像のパス
  $baceImagePath = $project.'pic/'.$file;
  // 画像保存先のパス(ファイル名)
  $saveImagePath = $project.'resized/Photo_'.sprintf('%04d', $num+1).'.png';
  echo 'convert thumb '.$file." to ".$saveImagePath."\n";
  // インスタンスを生成
  $image = new Imagick($baceImagePath);
  $image->resizeImage(1020, 512, Imagick::FILTER_LANCZOS, 0);
  // 保存
  $image->writeImage($saveImagePath);
}
?>
