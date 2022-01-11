github網址：https://github.com/limaoxd/unity_project

FollowPosition.cs: 讓物品跟著滑鼠移動
Inventory.cs: 記錄角色身上所持有的物品，控制背包的顯示與否
Item.cs: 物品的class，記錄物品的所有屬性
ItemDataBase.cs: 從Item.json檔取出所有儲存的物品列表，以備遊戲時從這邊取得物品的資訊
MainUIWeapon.cs: 遊戲畫面左下角控制當前角色所持武器
Potion.cs: 遊戲畫面左下角控制角色身上所剩藥水
Tooltip.cs: 背包欄滑鼠移至物品上會顯示他的資訊
UIInventory.cs: 記錄角色身上所持有的物品，並控制背包內左方的欄位的顯示
UIItem.cs: 物品在背包內的UI，控制物品的換位
death_effect.cs: 更改角色死亡的鏡頭特效
MainMenu.cs: 主選單控制
Mask.cs: 遮罩背包內配戴中的武器
Restart_button.cs: 重新設定角色狀態，場面整理
Boss_gate.cs: 偵測boss是否死亡，應用於所有boss（複數個也可）
Interaction.cs: 與場景中的物件互動
LevelChanger.cs: 場景切換跟淡入淡出的特效
life_time.cs: 計時並刪除物件
MainMenuMusic.cs: 主畫面音樂
spawner.cs: 敵人的重生點
LoadGame.cs: 載入遊戲
SaveGame.cs: 儲存遊戲
atk_trigger.cs: 判斷攻擊是否合法，用於角色和所有敵人
bar_on_head.cs: 讓小兵的血條顯示在頭上
dfc_trigger.cs: 判斷敵人的盾牌是否有擋住角色的攻擊
Level.cs: 升級系統、屬性值
Sound_emitter.cs: 讓動畫控制音效的生成
spell.cs: 發動遠程技能，僅限敵人
ThirdPersonController.cs: 角色所有的邏輯控制
Weapon_art.cs: 角色當前配戴的裝備，更改角色拿不同種類武器的動作
AI.cs: 給繼承的控制敵人的class
以下為所有BOSS與小怪的scripts:
AI_Ancient.cs
AI_Bastion.cs
AI_Bearclaw.cs
AI_Decimus.cs
AI_Falcius_Elite.cs
AI_Falcius_Spear.cs
AI_Falcius.Sword.cs
AI_Goldenking.cs
AI_Leotens.cs