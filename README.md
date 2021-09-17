# Graphics Buffer VFX Sample

## About

![gif](./dccs/../docs/kinect-vfx-graphicsbuffer.,p4.gif)

Unity 2021.2~使えるVisual Effect Graph 12では
Graphics Bufferをプロパティとして入力し、
Sample　Graphics Bufferノードで受け取ることができます。

Azure Kinectの点群をVFX Graphで表示するサンプルを作り直して、
GraphicsBufferで受け渡せるようにしました。

## Environment

- Unity 2021.2.0b9
- HDRP 12.0
- VFX Graph 12.0
- Windows 10 Home
- GeForce GTX 1060 3GB

## Install

VSのソリューションファイルを開いてnugetパッケージの復元を行うか、以下のコマンドを実行します。

```bash
# install
nuget install packages.config

# move dll files into unity project
./moveFiles.bat
```

プロジェクトを開いたときに、dllの名前衝突が起こる可能性があります。
Unity Searcherで使っているdllとAzure Kinect Sensor SDKで使っているdllが衝突してしまっているので、
PackageCacheから手動でUnity Searcherのcllをハードデリートすることにより解決しますが、自己責任でお願いいたします。

## Usage

Azure Kinectを接続した状態で、
`Assets\GraphicsBufferVFXSample\Scenes\main.unity`を再生します。

## Contact

何かございましたら、[にー兄さんのTwitter](https://twitter.com/ninisan_drumath)
までよろしくお願いいたします。
