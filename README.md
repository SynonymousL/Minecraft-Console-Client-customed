Minecraft Console Client
========================

[![Appveyor build status](https://ci.appveyor.com/api/projects/status/github/ORelio/Minecraft-Console-Client?branch=Indev)](https://ci.appveyor.com/project/ORelio/minecraft-console-client)

Minecraft Console Client(MCC) is a lightweight app allowing you to connect to any Minecraft server,
send commands and receive text messages in a fast and easy way without having to open the main Minecraft game. It also provides various automation for administration and other purposes.

## Looking for maintainers

Due to no longer having time to implement upgrades for new Minecraft versions and fixing bugs, I'm looking for motivated people to take over the project. If you feel like it could be you, please have a look at the [issues](https://github.com/ORelio/Minecraft-Console-Client/issues?q=is%3Aissue+is%3Aopen+label%3Awaiting-for%3Acontributor) section :)

## Download

Get exe file from the latest [development build](https://ci.appveyor.com/project/ORelio/minecraft-console-client/build/artifacts).
This exe file is a .NET binary which also works on Mac and Linux.

## How to use

Check out the [sample configuration files](MinecraftClient/config/) which includes the how-to-use README.
Help and more info is also available on the [Minecraft Forum thread](http://www.minecraftforum.net/topic/1314800-/).<br/>

## How to contribute

If you'd like to contribute to Minecraft Console Client, great, just fork the repository and submit a pull request. The *Indev* branch for contributions to future stable versions is no longer used as MCC is currently distributed as development builds only.

## License

Unless specifically stated, the code is from the MCC developers, and available under CDDL-1.0.
Else, the license and original author are mentioned in source file headers.
The main terms of the CDDL-1.0 license are basically the following:

- You may use the licensed code in whole or in part in any program you desire, regardless of the license of the program as a whole (or rather, as excluding the code you are borrowing). The program itself may be open or closed source, free or commercial.
- However, in all cases, any modifications, improvements, or additions to the CDDL code (any code that is referenced in direct modifications to the CDDL code is considered an addition to the CDDL code, and so is bound by this requirement; e.g. a modification of a math function to use a fast lookup table makes that table itself an addition to the CDDL code, regardless of whether it's in a source code file of its own) must be made publicly and freely available in source, under the CDDL license itself.
- In any program (source or binary) that uses CDDL code, recognition must be given to the source (either project or author) of the CDDL code. As well, modifications to the CDDL code (which must be distributed as source) may not remove notices indicating the ancestry of the code.

More info at http://qstuff.blogspot.fr/2007/04/why-cddl.html
Full license at http://opensource.org/licenses/CDDL-1.0

---

## 従来の仕様からの変更点
・MC1.13以降の対応
・複数アカウント、複数サーバーの情報を外部テキストファイルに書いておくと、セレクトボックスで読み込める機能を追加
（ファイルがない場合は従来の機能のみを使えるよう配慮）

## 既知の不具合
・ファイル書式エラーダイアログの行数表示がおかしい（文字列結合している）→後日修正
・GUI経由で全角文字を入力するとサーバー側で文字化けする（CUIでは問題ないため、入力メソッド回りの問題）

## 利用方法
### 1. MinecraftClientプロジェクト、MinecraftClientGUIプロジェクトをビルド
　デバッグビルドで構いません。リリースビルドはうまくいかないと思います。
  ビルド前に、プロジェクトプロパティから、デバッグ時の引数指定がないことを確認して下さい。

### 2. MinecraftClient/bin/Debug/下の下記ファイルをコピー、MinecraftClientGUI/bin/Debug/下にペースト
　MinecraftClient.exe
　MinecraftClient.ini

### 3. MinecraftClient.iniを編集
　下記項目を編集して下さい。
　　16行目 language=ja_JP    　　　　... 念の為
  　22行目 mcversion=1.13.2　　　　　... これは利用したいバージョンに合わせて下さい（必須）
  　33行目 accountlist=accounts.txt ... 任意（利用する場合のみ）
  　34行目 serverlist=servers.txt   ... 任意（利用する場合のみ）
  　62行目 locale=ja_JP             ... 念の為
  　67行目以降 botの利き手やスキンの上着の表示設定はお好みで。
  　上記以外の項目は弄らなくても動きます。
   
### 4. （任意）MinecraftClient/bin/Debug/下に accounts.txt と servers.txt を作成
　複数のアカウント情報、複数のサーバー情報を登録する予定がない場合や、
　毎回直接入力する形式でも構わない場合はこの作業は不要です。

### 5. （任意）accounts.txt と servers.txt を編集
　いずれも、テキストエディタで新しいファイルを作成して下さい。
　accounts.txtは1行に「表示ユーザー名」「メールアドレス」「パスワード」をカンマ区切りで記載、
　servers.txtは1行に「サーバーアドレス：ポート番号」のみを記載して下さい。
　ポート番号は書かなくても、iniファイルでデフォルトの25565番が読み込まれますが、
　デフォルトと異なるポート番号の場合は指定が必要です。

### 6. MinecraftClientGUI.exeを起動
　起動時にエラーダイアログが出る場合はaccounts.txtやservers.txtに何らかの不備があります。
　両方ともファイルを作成しなかった場合はエラーにならず、直接入力のテキストボックスのみ表示されます。
　起動後にテキストファイルを編集・削除しても、起動時に内容を読み込んでいるため、実行結果には反映されません。
　起動後に編集・削除した内容を反映させたい場合は、exeを立ち上げなおして下さい。

## その他
　実行ディレクトリに /lang/ja_JP.langがないと、一部システム内日本語表示ができない恐れがあります。
　不具合が生じた場合は、ja_JP.langがあるか確認してください。
　言うまでもなく、本プログラムの利用は自己責任にてお願いします。
　あくまでも私個人の利用を目的としているため、第三者からの改修・機能追加依頼は受け付けておりません。
　また、利用に伴う不利益等の責任は負いかねますので、予めご了承下さい。
 
2019.10.26 紅楽 梨莉
