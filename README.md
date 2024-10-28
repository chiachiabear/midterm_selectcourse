<h1>資料庫期中-選課系統</h1>
使用ASP.Net開發的Web，資料庫使用SQL Server

功能：
<ol>
<li>登入(無註冊，所有學生登入資料預先匯入)</li>
<li>登出</li>
<li>顯示已選課表</li>
<li>以選課代碼查詢課程</li>
<li>以科系年級查詢課程</li>
<li>以開課時程查詢課程</li>
<li>以科目名稱查詢課程</li>
<li>以開課老師查詢課程</li>
</ol>

特點：
 - 若加選課程跟已選課程撞時間，不能加選
 - 若加選課程跟已選課程撞名(不同班)，不能加選
 - 若加選課程已滿，不能加選
 - 退選最低不能低於9學分

以上SQL指令細節於midterm_selectcourse/Models/DBmanager.cs內

---

以下為部分截圖，其他功能截圖於/screenShots內

![登入](/screenShots/login.png "登入頁面")

![主頁面](/screenShots/mainPage.png "主頁面")

![快捷顯示課表](/screenShots/schedule.png "快捷顯示課表")

![以時段搜尋課程](/screenShots/weekSearch.png "以時段搜尋課程")


