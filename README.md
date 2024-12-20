# nedo-remote-rehabilitation-client

## Requriements
### Software
- Unity 2022.2.19f1
    - Android Build Support module
- Oculusアプリ
- Side Questアプリ

### Hardware
- Oculus Quest Pro または Oculus Quest 2 (これらの2種類の実機では動作確認をしました。)
- Linkケーブル
- Google Cloudと通信可能なWifi環境（セキュリティの厳しいWifi環境からはアクセスできない可能性があります。）

## Usage
### PCとリンクケーブルをつないだまま動作させる場合
1. OculusアプリとLinkケーブルを使ってHMDをPCにQuest Linkで接続してください。
1. Unityでプロジェクトを開いてください。
1. Assets/Secret.cs.exampleの内容を複製して、Assets/Secret.csを作成してください。
1. Secret.csの内容を適切な値に書き換えてください。
1. Unity Editor上でAsset/Scenes/Startのシーンを開いてください。
1. Playボタンを押してシステムを起動させてください。
1. 操作方法については[こちら](#操作方法)を参照してください。

### HMDのスタンドアローンで動作させる場合
1. Unityでプロジェクトを開いてください。
1. Assets/Secret.cs.exampleの内容を複製して、Assets/Secret.csを作成してください。
1. Secret.csの内容を適切な値に書き換えてください。
1. File / Build SettingsからBuild Settingsを開いてください。
1. Buildを押してビルドを実行してください。
1. .apkファイルがビルドされたら、ファイルを任意の場所に保存してください。
1. OculusアプリとLinkケーブルを使ってHMDをPCにQuest Linkで接続してください。
1. Side Questを使ってHMDに.apkファイルをインストールさせてください。
1. Linkケーブルを外して、HMD上でアプリケーションを選択して、システムを起動させてください。
1. 操作方法については[こちら](#操作方法)を参照してください。


### 操作方法
#### Startシーンにおける操作方法
最初にユーザー体験する白い地面に1つUIが表示されているシーンがStartシーンです。このシーンではリハビリテーションのモードやアバターを選択します。

| 操作項目 | 方法 |
| ---- | ---- |
| アバターの選択 | UI上のアバターのイラストにレイを合わせた状態で人差し指でコントローラーのトリガーを引くとアバターを選択することができます。 |
| モードの選択 | UI上の条件A, 条件B, 条件Cのボタンのどれかにレイを合わせた状態で人差し指でコントローラーのトリガーを引くとリハビリテーションのモードを選択することができます。条件Aはシンプルなシーンでリハビリテーションを行うモード、条件Bはゲーミフィケーションの要素を追加したモード、条件Cはさらにコミュニケーションの要素を追加したモードです。 |
| 移動 | 左右のコントローラーのスティックを使ってカメラの方向を回転させたり位置を移動させたりすることができます。 |

#### Rehabilitationシーンにおける操作方法
Startシーンでモードを選択すると10秒程度でRehabilitationシーンが読み込まれます。このシーンではユーザーはリハビリを行います。

| 操作項目 | 方法 |
| ---- | ---- |
| 高さ調整 | Aボタンを押すとアバターの高さを調整することができます。 |
| 移動 | 左右のコントローラーのスティックを使ってカメラの方向を回転させたり位置を移動させたりすることができます。 |
| リハビリ開始 / 停止 | VR空間内のテーブルの周辺にある水色の直方体のエリアに入った状態で、右手の人差し指でコントローラーのトリガーを引くとリハビリを開始できます。なお、再度トリガーを引くとリハビリを停止できます。 |
| リハビリの継続 | 右手のコントローラーを前方の離れた位置に置いて、左手のコントローラーを近づけたり遠ざけたりするように雑巾がけ挙上動作のリハビリテーションを行うことでリハビリテーションを継続することができます。 |
| リーチングの調整 | 雑巾がけ挙上動作における両手を最も手前に引いた位置でAボタンを、最も奥に押した位置でBボタンを押すと、リーチングが適切にリダイレクションされるようになります。 |
| 終了 | Xボタンを押すと終了のためのUIが表示されます。 |

