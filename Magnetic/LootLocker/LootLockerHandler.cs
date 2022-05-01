using Godot;
using System;

public class LootLockerHandler : Node
{
    HTTPRequest httpRequest;

    const string GameKey = "c9ee5b9d357d772451f303965e53a603b3b192c9";
    public static string playerName;
    string sessionToken;
    string playerIdentifier;
    int playerID;
    bool isFreshUser;

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.PlayerPrefsInited += OnPlayerPrefsInited;
        GameEvents.PlayerSetName += OnPlayerSetName;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.PlayerPrefsInited -= OnPlayerPrefsInited;
        GameEvents.PlayerSetName -= OnPlayerSetName;
    }

    void OnPlayerPrefsInited()
    {
        return;
        CheckIfFreshUser();
    }

    void OnPlayerSetName(string name)
    {
        playerName = name;
        SetPlayerName(playerName);
    }


    public override void _Ready()
    {
        GD.PrintErr("LL node entered sceneTree");
        httpRequest = GetNode<HTTPRequest>("HTTPRequest");
    }

    public void CheckIfFreshUser()
    {
        GD.PrintErr("LL chekcing if fresh user");
        playerIdentifier = PlayerPrefs.GetString("playerIdentifier");
        if(string.IsNullOrEmpty(playerIdentifier))
        {
            GD.Print("LL starting as fresh user");
            isFreshUser = true;
        }
        InitSession();
    }

    public void InitSession()
    {
        GD.PrintErr("LL initing seasion");
        string[] headers = {"Content-Type: application/json",};
        string url = "https://qyxw6316.api.lootlocker.io/game/v2/session/guest";
        string data = $"{{ \"game_key\":\"{GameKey}\", \"game_version\": \"0.10.0.0\", \"development_mode\": \"false\"}}";

        if(!isFreshUser)
        {
            data = $"{{ \"game_key\":\"{GameKey}\", \"game_version\": \"0.10.0.0\", \"development_mode\": \"false\", \"player_identifier\":\"{playerIdentifier}\"}}";
        }

        httpRequest.Connect("request_completed", this, nameof(OnInitSessionCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Post, data);
    }

    public void OnInitSessionCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            Godot.Collections.Dictionary results = json.Result as Godot.Collections.Dictionary;
            sessionToken = results["session_token"].ToString();
            playerID = Int32.Parse(results["player_id"].ToString());
            playerIdentifier = results["player_identifier"].ToString();

            //Save playerIdentifier in cookie so if player reloads he does not need to type name again
            PlayerPrefs.SetString("playerIdentifier", playerIdentifier);

            GD.PrintErr("LL initing seasion completed");
        }else
        {
            GD.PrintErr("LL failed to make seasion");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnInitSessionCompleted));

        if(isFreshUser)
        {
            GameEvents.ShowNameSetterPopup?.Invoke();            
        }else
        {
            GetPlayerName();
        }
    }


    public void EndSession()
    {
        GD.PrintErr("LL ending seasion");
        string[] headers = {"Content-Type: application/json", $"x-session-token: {sessionToken}"};
        string url = "https://qyxw6316.api.lootlocker.io/game/v1/session";
        httpRequest.Connect("request_completed", this, nameof(OnEndSessionCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Delete);
    }

    public void OnEndSessionCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL ending seasion completed");
        }else
        {
            GD.PrintErr("LL failed to end seasion");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnEndSessionCompleted));
    }

    public void GetPlayerName()
    {
        GD.PrintErr("LL getting name");
        string[] headers = {"Content-Type: application/json", $"x-session-token: {sessionToken}"};
        string url = "https://qyxw6316.api.lootlocker.io/game/player/name";
        httpRequest.Connect("request_completed", this, nameof(OnGetPlayerNameCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Get);
    }

    public void OnGetPlayerNameCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL name get completed");
            Godot.Collections.Dictionary results = json.Result as Godot.Collections.Dictionary;
            playerName = results["name"].ToString();
        }else
        {
            GD.PrintErr("LL failed to get name");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnSetPlayerNameCompleted));

        GameEvents.ShowStartScene?.Invoke();
    }

    public void SetPlayerName(string name)
    {
        GD.PrintErr("LL setting name");
        string[] headers = {"Content-Type: application/json", $"x-session-token: {sessionToken}"};
        string url = "https://qyxw6316.api.lootlocker.io/game/player/name";
        string data = $"{{ \"name\":\"{name}\"}}";
        httpRequest.Connect("request_completed", this, nameof(OnSetPlayerNameCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Patch, data);
    }

    public void OnSetPlayerNameCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL name set completed");
            Godot.Collections.Dictionary results = json.Result as Godot.Collections.Dictionary;
            playerName = results["name"].ToString();
            PlayerPrefs.SetString("playerName", playerName);
        }else
        {
            GD.PrintErr("LL failed to set name");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnSetPlayerNameCompleted));
        GameEvents.ShowStartScene?.Invoke();
    }

    public void SendScore(int score)
    {
        GD.PrintErr("LL sending score");
        string[] headers = {"Content-Type: application/json", $"x-session-token: {sessionToken}"};
        string url = "https://qyxw6316.api.lootlocker.io/game/leaderboards/2666/submit";
        string data = $"{{ \"score\":{score}}}";
        httpRequest.Connect("request_completed", this, nameof(OnSendScoreCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Post, data);
    }

    public void OnSendScoreCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL score send completed");
        }else
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL failed to send score");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnSendScoreCompleted));
    }

    public void GetLeaderboardTop10Data()
    {
        GD.PrintErr("LL getting top 10");
        string[] headers = {"Content-Type: application/json", $"x-session-token: {sessionToken}"};
        string url = "https://qyxw6316.api.lootlocker.io/game/leaderboards/2666/list?count=10";
        httpRequest.Connect("request_completed", this, nameof(OnGetLeaderboardTop10DataCompleted));
        httpRequest.Request(url, headers, false, HTTPClient.Method.Get);
    }

    public void OnGetLeaderboardTop10DataCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if(response_code==200)
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL got top 10 completed");
        }else
        {
            JSONParseResult json = JSON.Parse(System.Text.Encoding.UTF8.GetString(body));
            GD.Print(json.Result);
            GD.PrintErr("LL failed to get top 10");
        }
        httpRequest.Disconnect("request_completed", this, nameof(OnGetLeaderboardTop10DataCompleted));
    }

    void SaveData()
    {

    }
}
