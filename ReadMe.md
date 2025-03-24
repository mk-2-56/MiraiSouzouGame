コードレビュー担当者さま

レビューして頂けて光栄で、ありがとうございます。

チームがUnityのバージョン管理に慣れていないため、
branchが混乱で、途中からpackage受け渡しなどにより、リポジトリが少しごちゃごちゃになっており、申し訳ありません。
以下のGithubとbranchからのコードレビューをお願いできればありがたいです。

GitHub リポジトリ:
    https://github.com/mk-2-56/MiraiSouzouGame
    最新Branch: chousei

Unity Editorバージョン:2022.3.50f1

今後ともご指導、ご鞭撻よろしくお願いいたします。
ウ皓



主な担当箇所:

    C#:
        1.Assets\Scripts\Cameraフォルダー
            ・GameCameraManager(約55%):             プレイヤー加入時のGameCamera生成および管理

        2.Assets\Scripts\Gimmickフォルダー
            ・BoostTile:                            加速板の処理

        3.Assets\Scripts\Effect
            ・SurfEffectSwitcher:                   プレイヤーの移動VFXを何種類中で制御して切り替えるモジュール

        4.Assets\Scripts\PlayerCharacter-ukohフォルダー
            ・Tactileフォルダー
                CC　(Character Controller) namespace
                    Hub:                            Unity Input System組み込み。入力や他eventの発行、他のシステムとのやり取り
                    Basic:                          プレイヤー基本移動
                    Hover:                          プレイヤーRB浮遊機能、壁、天井で走る時の付着処理
                    Drift:                          プレイヤードリフト機能
            ・LaunchSystemフォルダー
                CC　(Character Controller) namespace
                    Dash:                           プレイヤーの追跡Dash機能
                TargetManager:                      プレイヤーの追跡Dash機能の標的管理

        5.Assets\Scripts\util-ukoh
            ・Debug:                                他のclassから呼び出して、メッセージを毎フレイムcanvasに表示
            ・Fader:                                他のclassから呼び出して、float数値をfadeout、fadeinする
            ・GameCamera:　                         Gamemodeプレイヤーカメラ、自動補間追尾など
            ・AU namespace
                Math:                               UnityにいないSmoothStepの実装だけ
            ・PlayerEffectDispatcher:               effect関連なコードに向けて、プレイヤーに関連するeventを発行
            ・PlayerSFXController:                  PlayerEffectDispatcherからSFX関連のやつを受理して統一的に処理する
            ・SplineRider:                          (未使用)Splineの方向に合わせて物理系オブジェを加速させる
            ・TargetUI:                             追跡Dash機能の標的用、GUIにoverlay表示させるとそのSpriteのobject poll
            ・TrackPositionManager:                 Splineに沿ってプレイヤーの進行状態を算出およびランク付けする
            ・TrailContoller:                       風を切る、プレイヤーが走った土煙などを制御する

        6.Assets\Scripts\System
            ・PlayerManager(約70%):                 プレイヤー生成、管理などの部分

    そのほか:
        1.Player、Game Cameraなど、関連Objectの設計
        2.Layerの設定
        3.一部VFX、Shaderの組み込み、調整
            組み込み、調整:
                しぶき、土煙…
                風切り
                プレイヤーの航跡
            調整:
                集中線

        4.火山洞窟Stageのライティングなど