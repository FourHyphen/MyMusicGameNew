# Description
- 機能
  - いわゆる音楽ゲームの基盤です
- 開発の目的
  - 自身の経験のため
    - BDDを試しました
    - 非同期処理のテスト自動化を実践しました
- アピールポイント
  - 非同期処理でもたいていの振る舞いテストを自動化しました
  - 振る舞いテストを出来るだけ簡単に書けるよう工夫しました
    - テストAPI(Codeer.Friendly)制御用ラッパを、なるべくユーザーの操作概念と一致するよう実装しました
- 反省点
  - 振る舞いのテスト自動化に集中し過ぎて単体テストが不十分になりました
    - 振る舞いのテストをトップダウンでまず記載してレッドを確認し、その後ボトムアップを意識して単体テストのレッド→グリーン→リファクタを回すべきでした
    - この点において、真にBDDを実践できたとは言い難い結果となりました
- 注意事項
  - 楽しむためのゲームの作成ではありません
    - 楽しむためのコンテンツは何一つありません、全て動作確認用の最低限のコンテンツになります
  - 高速化はテストに支障が生じた際に最低限を実施したのみになります。記憶の限りでは高速化は1回だけ実施しました

# Requirement
- ビルドに必要な環境
  - Visual Studio
  - .NET Framework 4.5
- Nuget 取得Package
  - JSONファイル処理
    - Newtonsoft.Json 12.0.3
  - テスト
    - MSTest
      - MSTest.TestAdapter 2.1.2
      - MSTest.TestFramework 2.1.2
    - 自動化API
      - Codeer.Friendly 2.6.1
      - Codeer.Friendly.Windows 2.15.0
      - Codeer.Friendly.Windows.Grasp 2.12.0
      - RM.Friendly.WPFStandardControls 1.41.2

# Usage

## 用語
- ノート
  - ゲームプレイ時に判定ラインに向かって移動してくるもののこと

## 1プレイの流れ

```
exe起動後、まず曲選択画面が表示されます
↓
画面左上、曲リストの中から曲を選択
  ・画面左下にこれまでのベスト結果が表示されます
↓
必要に応じて、画面右上のオプションを指定
  ・direction of note : ノートの移動方向指定
    ・top to bottom -> 画面上部から下方向に移動します
    ・right to left -> 画面右側から左方向に移動します
  ・rate of note's speed : ノードの移動速度倍率
    ・0.5倍 ～ 3.0倍まで選択できます
↓
ゲームスタート
  ・開始前にカウントダウンが入ります
↓
ゲームプレイ中
  ・ノートは2つのレーンを移動してきます
    ・TopToBottomモードの場合 -> 画面左側と右側
    ・RightToLeftモードの場合 -> 画面上側と下側
  ・ノートを拾う際、判定ラインとの近さに応じて Perfect、Good、Bad のいずれかが判定されます
    ・ノートを拾わずに見逃した場合はBad判定となります
  ・ノートの拾い方
    ・TopToBottomモードの場合 -> 画面左側は"f"キー、右側が"j"キー
    ・RightToLeftモードの場合 -> 画面上側は"f"キー、下側が"j"キー
    ・いずれのモードの場合でも左クリックで拾うこともできます
  ・ゲームプレイ中、"enter"キーを押すことで一時停止できます
    ・"Please press for restart"ボタンを左クリックすることで再開します
      ・再開時にカウントダウンが入ります
    ★一時停止したままexeを終了すると、それまでのプレイ結果はなかったことになりますのでご注意ください
↓
ゲーム完了
　・結果画面が表示されます
  ・得点はPerfectが2点、Goodが1点、Badが0点として、全ノートの判定結果を加算したものになります
  ・結果画面で"OK"ボタンを押すと曲選択画面に戻ります
```

# Install

2021/2/16時点でコード公開のみ実施しています。お手数ですがご自身でビルドしてください。

# data
 - 曲
   - フリーBGM Music with myuu byみゅうー
     - http://www.ne.jp/asahi/music/myuu/wave/wave.htm
 - 効果音
   - 魔王魂
     - https://maoudamashii.jokersounds.com/

# Licence

- [MIT](https://github.com/tcnksm/tool/blob/master/LICENCE)
