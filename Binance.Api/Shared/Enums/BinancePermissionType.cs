namespace Binance.Api.Shared;

/// <summary>
/// Type of account
/// </summary>
public enum BinancePermissionType : int
{
    /// <summary>
    /// Spot trading
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary>
    /// Margin trading
    /// </summary>>
    [Map("MARGIN")]
    Margin,

    /// <summary>
    /// Futures trading
    /// </summary>
    [Map("FUTURES")]
    Futures,

    /// <summary>
    /// Leveraged trading
    /// </summary>
    [Map("LEVERAGED")]
    Leveraged,

    /// <summary>
    /// Pre-market trading
    /// </summary>
    [Map("PRE_MARKET")]
    PreMarket,

    /// <summary>
    /// Trade group 1
    /// </summary>
    [Map("TRD_GRP_001")]
    TradeGroup001 = 101,

    /// <summary>
    /// Trade group 2
    /// </summary>
    [Map("TRD_GRP_002")]
    TradeGroup002,

    /// <summary>
    /// Trade group 3
    /// </summary>
    [Map("TRD_GRP_003")]
    TradeGroup003,

    /// <summary>
    /// Trade group 4
    /// </summary>
    [Map("TRD_GRP_004")]
    TradeGroup004,

    /// <summary>
    /// Trade group 5
    /// </summary>
    [Map("TRD_GRP_005")]
    TradeGroup005,

    /// <summary>
    /// Trade group 6
    /// </summary>
    [Map("TRD_GRP_006")]
    TradeGroup006,

    /// <summary>
    /// Trade group 7
    /// </summary>
    [Map("TRD_GRP_007")]
    TradeGroup007,

    /// <summary>
    /// Trade group 8
    /// </summary>
    [Map("TRD_GRP_008")]
    TradeGroup008,

    /// <summary>
    /// Trade group 9
    /// </summary>
    [Map("TRD_GRP_009")]
    TradeGroup009,

    /// <summary>
    /// Trade group 10
    /// </summary>
    [Map("TRD_GRP_010")]
    TradeGroup010,

    /// <summary>
    /// Trade group 11
    /// </summary>
    [Map("TRD_GRP_011")]
    TradeGroup011,

    /// <summary>
    /// Trade group 12
    /// </summary>
    [Map("TRD_GRP_012")]
    TradeGroup012,

    /// <summary>
    /// Trade group 13
    /// </summary>
    [Map("TRD_GRP_013")]
    TradeGroup013,

    /// <summary>
    /// Trade group 14
    /// </summary>
    [Map("TRD_GRP_014")]
    TradeGroup014,

    /// <summary>
    /// Trade group 15
    /// </summary>
    [Map("TRD_GRP_015")]
    TradeGroup015,

    /// <summary>
    /// Trade group 16
    /// </summary>
    [Map("TRD_GRP_016")]
    TradeGroup016,

    /// <summary>
    /// Trade group 17
    /// </summary>
    [Map("TRD_GRP_017")]
    TradeGroup017,

    /// <summary>
    /// Trade group 18
    /// </summary>
    [Map("TRD_GRP_018")]
    TradeGroup018,

    /// <summary>
    /// Trade group 19
    /// </summary>
    [Map("TRD_GRP_019")]
    TradeGroup019,

    /// <summary>
    /// Trade group 20
    /// </summary>
    [Map("TRD_GRP_020")]
    TradeGroup020,

    /// <summary>
    /// Trade group 21
    /// </summary>
    [Map("TRD_GRP_021")]
    TradeGroup021,

    /// <summary>
    /// Trade group 22
    /// </summary>
    [Map("TRD_GRP_022")]
    TradeGroup022,

    /// <summary>
    /// Trade group 23
    /// </summary>
    [Map("TRD_GRP_023")]
    TradeGroup023,

    /// <summary>
    /// Trade group 24
    /// </summary>
    [Map("TRD_GRP_024")]
    TradeGroup024,

    /// <summary>
    /// Trade group 25
    /// </summary>
    [Map("TRD_GRP_025")]
    TradeGroup025,

    /// <summary>
    /// Trade group 26
    /// </summary>
    [Map("TRD_GRP_026")]
    TradeGroup026,

    /// <summary>
    /// Trade group 27
    /// </summary>
    [Map("TRD_GRP_027")]
    TradeGroup027,

    /// <summary>
    /// Trade group 28
    /// </summary>
    [Map("TRD_GRP_028")]
    TradeGroup028,

    /// <summary>
    /// Trade group 29
    /// </summary>
    [Map("TRD_GRP_029")]
    TradeGroup029,

    /// <summary>
    /// Trade group 30
    /// </summary>
    [Map("TRD_GRP_030")]
    TradeGroup030,

    /// <summary>
    /// Trade group 31
    /// </summary>
    [Map("TRD_GRP_031")]
    TradeGroup031,

    /// <summary>
    /// Trade group 32
    /// </summary>
    [Map("TRD_GRP_032")]
    TradeGroup032,

    /// <summary>
    /// Trade group 33
    /// </summary>
    [Map("TRD_GRP_033")]
    TradeGroup033,

    /// <summary>
    /// Trade group 34
    /// </summary>
    [Map("TRD_GRP_034")]
    TradeGroup034,

    /// <summary>
    /// Trade group 35
    /// </summary>
    [Map("TRD_GRP_035")]
    TradeGroup035,

    /// <summary>
    /// Trade group 36
    /// </summary>
    [Map("TRD_GRP_036")]
    TradeGroup036,

    /// <summary>
    /// Trade group 37
    /// </summary>
    [Map("TRD_GRP_037")]
    TradeGroup037,

    /// <summary>
    /// Trade group 38
    /// </summary>
    [Map("TRD_GRP_038")]
    TradeGroup038,

    /// <summary>
    /// Trade group 39
    /// </summary>
    [Map("TRD_GRP_039")]
    TradeGroup039,

    /// <summary>
    /// Trade group 40
    /// </summary>
    [Map("TRD_GRP_040")]
    TradeGroup040,

    /// <summary>
    /// Trade group 41
    /// </summary>
    [Map("TRD_GRP_041")]
    TradeGroup041,

    /// <summary>
    /// Trade group 42
    /// </summary>
    [Map("TRD_GRP_042")]
    TradeGroup042,

    /// <summary>
    /// Trade group 43
    /// </summary>
    [Map("TRD_GRP_043")]
    TradeGroup043,

    /// <summary>
    /// Trade group 44
    /// </summary>
    [Map("TRD_GRP_044")]
    TradeGroup044,

    /// <summary>
    /// Trade group 45
    /// </summary>
    [Map("TRD_GRP_045")]
    TradeGroup045,

    /// <summary>
    /// Trade group 46
    /// </summary>
    [Map("TRD_GRP_046")]
    TradeGroup046,

    /// <summary>
    /// Trade group 47
    /// </summary>
    [Map("TRD_GRP_047")]
    TradeGroup047,

    /// <summary>
    /// Trade group 48
    /// </summary>
    [Map("TRD_GRP_048")]
    TradeGroup048,

    /// <summary>
    /// Trade group 49
    /// </summary>
    [Map("TRD_GRP_049")]
    TradeGroup049,

    /// <summary>
    /// Trade group 50
    /// </summary>
    [Map("TRD_GRP_050")]
    TradeGroup050,

    /// <summary>
    /// Trade group 51
    /// </summary>
    [Map("TRD_GRP_051")]
    TradeGroup051,

    /// <summary>
    /// Trade group 52
    /// </summary>
    [Map("TRD_GRP_052")]
    TradeGroup052,

    /// <summary>
    /// Trade group 53
    /// </summary>
    [Map("TRD_GRP_053")]
    TradeGroup053,

    /// <summary>
    /// Trade group 54
    /// </summary>
    [Map("TRD_GRP_054")]
    TradeGroup054,

    /// <summary>
    /// Trade group 55
    /// </summary>
    [Map("TRD_GRP_055")]
    TradeGroup055,

    /// <summary>
    /// Trade group 56
    /// </summary>
    [Map("TRD_GRP_056")]
    TradeGroup056,

    /// <summary>
    /// Trade group 57
    /// </summary>
    [Map("TRD_GRP_057")]
    TradeGroup057,

    /// <summary>
    /// Trade group 58
    /// </summary>
    [Map("TRD_GRP_058")]
    TradeGroup058,

    /// <summary>
    /// Trade group 59
    /// </summary>
    [Map("TRD_GRP_059")]
    TradeGroup059,

    /// <summary>
    /// Trade group 60
    /// </summary>
    [Map("TRD_GRP_060")]
    TradeGroup060,

    /// <summary>
    /// Trade group 61
    /// </summary>
    [Map("TRD_GRP_061")]
    TradeGroup061,

    /// <summary>
    /// Trade group 62
    /// </summary>
    [Map("TRD_GRP_062")]
    TradeGroup062,

    /// <summary>
    /// Trade group 63
    /// </summary>
    [Map("TRD_GRP_063")]
    TradeGroup063,

    /// <summary>
    /// Trade group 64
    /// </summary>
    [Map("TRD_GRP_064")]
    TradeGroup064,

    /// <summary>
    /// Trade group 65
    /// </summary>
    [Map("TRD_GRP_065")]
    TradeGroup065,

    /// <summary>
    /// Trade group 66
    /// </summary>
    [Map("TRD_GRP_066")]
    TradeGroup066,

    /// <summary>
    /// Trade group 67
    /// </summary>
    [Map("TRD_GRP_067")]
    TradeGroup067,

    /// <summary>
    /// Trade group 68
    /// </summary>
    [Map("TRD_GRP_068")]
    TradeGroup068,

    /// <summary>
    /// Trade group 69
    /// </summary>
    [Map("TRD_GRP_069")]
    TradeGroup069,

    /// <summary>
    /// Trade group 70
    /// </summary>
    [Map("TRD_GRP_070")]
    TradeGroup070,

    /// <summary>
    /// Trade group 71
    /// </summary>
    [Map("TRD_GRP_071")]
    TradeGroup071,

    /// <summary>
    /// Trade group 72
    /// </summary>
    [Map("TRD_GRP_072")]
    TradeGroup072,

    /// <summary>
    /// Trade group 73
    /// </summary>
    [Map("TRD_GRP_073")]
    TradeGroup073,

    /// <summary>
    /// Trade group 74
    /// </summary>
    [Map("TRD_GRP_074")]
    TradeGroup074,

    /// <summary>
    /// Trade group 75
    /// </summary>
    [Map("TRD_GRP_075")]
    TradeGroup075,

    /// <summary>
    /// Trade group 76
    /// </summary>
    [Map("TRD_GRP_076")]
    TradeGroup076,

    /// <summary>
    /// Trade group 77
    /// </summary>
    [Map("TRD_GRP_077")]
    TradeGroup077,

    /// <summary>
    /// Trade group 78
    /// </summary>
    [Map("TRD_GRP_078")]
    TradeGroup078,

    /// <summary>
    /// Trade group 79
    /// </summary>
    [Map("TRD_GRP_079")]
    TradeGroup079,

    /// <summary>
    /// Trade group 80
    /// </summary>
    [Map("TRD_GRP_080")]
    TradeGroup080,

    /// <summary>
    /// Trade group 81
    /// </summary>
    [Map("TRD_GRP_081")]
    TradeGroup081,

    /// <summary>
    /// Trade group 82
    /// </summary>
    [Map("TRD_GRP_082")]
    TradeGroup082,

    /// <summary>
    /// Trade group 83
    /// </summary>
    [Map("TRD_GRP_083")]
    TradeGroup083,

    /// <summary>
    /// Trade group 84
    /// </summary>
    [Map("TRD_GRP_084")]
    TradeGroup084,

    /// <summary>
    /// Trade group 85
    /// </summary>
    [Map("TRD_GRP_085")]
    TradeGroup085,

    /// <summary>
    /// Trade group 86
    /// </summary>
    [Map("TRD_GRP_086")]
    TradeGroup086,

    /// <summary>
    /// Trade group 87
    /// </summary>
    [Map("TRD_GRP_087")]
    TradeGroup087,

    /// <summary>
    /// Trade group 88
    /// </summary>
    [Map("TRD_GRP_088")]
    TradeGroup088,

    /// <summary>
    /// Trade group 89
    /// </summary>
    [Map("TRD_GRP_089")]
    TradeGroup089,

    /// <summary>
    /// Trade group 90
    /// </summary>
    [Map("TRD_GRP_090")]
    TradeGroup090,

    /// <summary>
    /// Trade group 91
    /// </summary>
    [Map("TRD_GRP_091")]
    TradeGroup091,

    /// <summary>
    /// Trade group 92
    /// </summary>
    [Map("TRD_GRP_092")]
    TradeGroup092,

    /// <summary>
    /// Trade group 93
    /// </summary>
    [Map("TRD_GRP_093")]
    TradeGroup093,

    /// <summary>
    /// Trade group 94
    /// </summary>
    [Map("TRD_GRP_094")]
    TradeGroup094,

    /// <summary>
    /// Trade group 95
    /// </summary>
    [Map("TRD_GRP_095")]
    TradeGroup095,

    /// <summary>
    /// Trade group 96
    /// </summary>
    [Map("TRD_GRP_096")]
    TradeGroup096,

    /// <summary>
    /// Trade group 97
    /// </summary>
    [Map("TRD_GRP_097")]
    TradeGroup097,

    /// <summary>
    /// Trade group 98
    /// </summary>
    [Map("TRD_GRP_098")]
    TradeGroup098,

    /// <summary>
    /// Trade group 99
    /// </summary>
    [Map("TRD_GRP_099")]
    TradeGroup099,

    /// <summary>
    /// Trade group 100
    /// </summary>
    [Map("TRD_GRP_100")]
    TradeGroup100,

    /// <summary>
    /// Trade group 101
    /// </summary>
    [Map("TRD_GRP_101")]
    TradeGroup101,

    /// <summary>
    /// Trade group 102
    /// </summary>
    [Map("TRD_GRP_102")]
    TradeGroup102,

    /// <summary>
    /// Trade group 103
    /// </summary>
    [Map("TRD_GRP_103")]
    TradeGroup103,

    /// <summary>
    /// Trade group 104
    /// </summary>
    [Map("TRD_GRP_104")]
    TradeGroup104,

    /// <summary>
    /// Trade group 105
    /// </summary>
    [Map("TRD_GRP_105")]
    TradeGroup105,

    /// <summary>
    /// Trade group 106
    /// </summary>
    [Map("TRD_GRP_106")]
    TradeGroup106,

    /// <summary>
    /// Trade group 107
    /// </summary>
    [Map("TRD_GRP_107")]
    TradeGroup107,

    /// <summary>
    /// Trade group 108
    /// </summary>
    [Map("TRD_GRP_108")]
    TradeGroup108,

    /// <summary>
    /// Trade group 109
    /// </summary>
    [Map("TRD_GRP_109")]
    TradeGroup109,

    /// <summary>
    /// Trade group 110
    /// </summary>
    [Map("TRD_GRP_110")]
    TradeGroup110,

    /// <summary>
    /// Trade group 111
    /// </summary>
    [Map("TRD_GRP_111")]
    TradeGroup111,

    /// <summary>
    /// Trade group 112
    /// </summary>
    [Map("TRD_GRP_112")]
    TradeGroup112,

    /// <summary>
    /// Trade group 113
    /// </summary>
    [Map("TRD_GRP_113")]
    TradeGroup113,

    /// <summary>
    /// Trade group 114
    /// </summary>
    [Map("TRD_GRP_114")]
    TradeGroup114,

    /// <summary>
    /// Trade group 115
    /// </summary>
    [Map("TRD_GRP_115")]
    TradeGroup115,

    /// <summary>
    /// Trade group 116
    /// </summary>
    [Map("TRD_GRP_116")]
    TradeGroup116,

    /// <summary>
    /// Trade group 117
    /// </summary>
    [Map("TRD_GRP_117")]
    TradeGroup117,

    /// <summary>
    /// Trade group 118
    /// </summary>
    [Map("TRD_GRP_118")]
    TradeGroup118,

    /// <summary>
    /// Trade group 119
    /// </summary>
    [Map("TRD_GRP_119")]
    TradeGroup119,

    /// <summary>
    /// Trade group 120
    /// </summary>
    [Map("TRD_GRP_120")]
    TradeGroup120,

    /// <summary>
    /// Trade group 121
    /// </summary>
    [Map("TRD_GRP_121")]
    TradeGroup121,

    /// <summary>
    /// Trade group 122
    /// </summary>
    [Map("TRD_GRP_122")]
    TradeGroup122,

    /// <summary>
    /// Trade group 123
    /// </summary>
    [Map("TRD_GRP_123")]
    TradeGroup123,

    /// <summary>
    /// Trade group 124
    /// </summary>
    [Map("TRD_GRP_124")]
    TradeGroup124,

    /// <summary>
    /// Trade group 125
    /// </summary>
    [Map("TRD_GRP_125")]
    TradeGroup125,

    /// <summary>
    /// Trade group 126
    /// </summary>
    [Map("TRD_GRP_126")]
    TradeGroup126,

    /// <summary>
    /// Trade group 127
    /// </summary>
    [Map("TRD_GRP_127")]
    TradeGroup127,

    /// <summary>
    /// Trade group 128
    /// </summary>
    [Map("TRD_GRP_128")]
    TradeGroup128,

    /// <summary>
    /// Trade group 129
    /// </summary>
    [Map("TRD_GRP_129")]
    TradeGroup129,

    /// <summary>
    /// Trade group 130
    /// </summary>
    [Map("TRD_GRP_130")]
    TradeGroup130,

    /// <summary>
    /// Trade group 131
    /// </summary>
    [Map("TRD_GRP_131")]
    TradeGroup131,

    /// <summary>
    /// Trade group 132
    /// </summary>
    [Map("TRD_GRP_132")]
    TradeGroup132,

    /// <summary>
    /// Trade group 133
    /// </summary>
    [Map("TRD_GRP_133")]
    TradeGroup133,

    /// <summary>
    /// Trade group 134
    /// </summary>
    [Map("TRD_GRP_134")]
    TradeGroup134,

    /// <summary>
    /// Trade group 135
    /// </summary>
    [Map("TRD_GRP_135")]
    TradeGroup135,

    /// <summary>
    /// Trade group 136
    /// </summary>
    [Map("TRD_GRP_136")]
    TradeGroup136,

    /// <summary>
    /// Trade group 137
    /// </summary>
    [Map("TRD_GRP_137")]
    TradeGroup137,

    /// <summary>
    /// Trade group 138
    /// </summary>
    [Map("TRD_GRP_138")]
    TradeGroup138,

    /// <summary>
    /// Trade group 139
    /// </summary>
    [Map("TRD_GRP_139")]
    TradeGroup139,

    /// <summary>
    /// Trade group 140
    /// </summary>
    [Map("TRD_GRP_140")]
    TradeGroup140,

    /// <summary>
    /// Trade group 141
    /// </summary>
    [Map("TRD_GRP_141")]
    TradeGroup141,

    /// <summary>
    /// Trade group 142
    /// </summary>
    [Map("TRD_GRP_142")]
    TradeGroup142,

    /// <summary>
    /// Trade group 143
    /// </summary>
    [Map("TRD_GRP_143")]
    TradeGroup143,

    /// <summary>
    /// Trade group 144
    /// </summary>
    [Map("TRD_GRP_144")]
    TradeGroup144,

    /// <summary>
    /// Trade group 145
    /// </summary>
    [Map("TRD_GRP_145")]
    TradeGroup145,

    /// <summary>
    /// Trade group 146
    /// </summary>
    [Map("TRD_GRP_146")]
    TradeGroup146,

    /// <summary>
    /// Trade group 147
    /// </summary>
    [Map("TRD_GRP_147")]
    TradeGroup147,

    /// <summary>
    /// Trade group 148
    /// </summary>
    [Map("TRD_GRP_148")]
    TradeGroup148,

    /// <summary>
    /// Trade group 149
    /// </summary>
    [Map("TRD_GRP_149")]
    TradeGroup149,

    /// <summary>
    /// Trade group 150
    /// </summary>
    [Map("TRD_GRP_150")]
    TradeGroup150,

    /// <summary>
    /// Trade group 151
    /// </summary>
    [Map("TRD_GRP_151")]
    TradeGroup151,

    /// <summary>
    /// Trade group 152
    /// </summary>
    [Map("TRD_GRP_152")]
    TradeGroup152,

    /// <summary>
    /// Trade group 153
    /// </summary>
    [Map("TRD_GRP_153")]
    TradeGroup153,

    /// <summary>
    /// Trade group 154
    /// </summary>
    [Map("TRD_GRP_154")]
    TradeGroup154,

    /// <summary>
    /// Trade group 155
    /// </summary>
    [Map("TRD_GRP_155")]
    TradeGroup155,

    /// <summary>
    /// Trade group 156
    /// </summary>
    [Map("TRD_GRP_156")]
    TradeGroup156,

    /// <summary>
    /// Trade group 157
    /// </summary>
    [Map("TRD_GRP_157")]
    TradeGroup157,

    /// <summary>
    /// Trade group 158
    /// </summary>
    [Map("TRD_GRP_158")]
    TradeGroup158,

    /// <summary>
    /// Trade group 159
    /// </summary>
    [Map("TRD_GRP_159")]
    TradeGroup159,

    /// <summary>
    /// Trade group 160
    /// </summary>
    [Map("TRD_GRP_160")]
    TradeGroup160,

    /// <summary>
    /// Trade group 161
    /// </summary>
    [Map("TRD_GRP_161")]
    TradeGroup161,

    /// <summary>
    /// Trade group 162
    /// </summary>
    [Map("TRD_GRP_162")]
    TradeGroup162,

    /// <summary>
    /// Trade group 163
    /// </summary>
    [Map("TRD_GRP_163")]
    TradeGroup163,

    /// <summary>
    /// Trade group 164
    /// </summary>
    [Map("TRD_GRP_164")]
    TradeGroup164,

    /// <summary>
    /// Trade group 165
    /// </summary>
    [Map("TRD_GRP_165")]
    TradeGroup165,

    /// <summary>
    /// Trade group 166
    /// </summary>
    [Map("TRD_GRP_166")]
    TradeGroup166,

    /// <summary>
    /// Trade group 167
    /// </summary>
    [Map("TRD_GRP_167")]
    TradeGroup167,

    /// <summary>
    /// Trade group 168
    /// </summary>
    [Map("TRD_GRP_168")]
    TradeGroup168,

    /// <summary>
    /// Trade group 169
    /// </summary>
    [Map("TRD_GRP_169")]
    TradeGroup169,

    /// <summary>
    /// Trade group 170
    /// </summary>
    [Map("TRD_GRP_170")]
    TradeGroup170,

    /// <summary>
    /// Trade group 171
    /// </summary>
    [Map("TRD_GRP_171")]
    TradeGroup171,

    /// <summary>
    /// Trade group 172
    /// </summary>
    [Map("TRD_GRP_172")]
    TradeGroup172,

    /// <summary>
    /// Trade group 173
    /// </summary>
    [Map("TRD_GRP_173")]
    TradeGroup173,

    /// <summary>
    /// Trade group 174
    /// </summary>
    [Map("TRD_GRP_174")]
    TradeGroup174,

    /// <summary>
    /// Trade group 175
    /// </summary>
    [Map("TRD_GRP_175")]
    TradeGroup175,

    /// <summary>
    /// Trade group 176
    /// </summary>
    [Map("TRD_GRP_176")]
    TradeGroup176,

    /// <summary>
    /// Trade group 177
    /// </summary>
    [Map("TRD_GRP_177")]
    TradeGroup177,

    /// <summary>
    /// Trade group 178
    /// </summary>
    [Map("TRD_GRP_178")]
    TradeGroup178,

    /// <summary>
    /// Trade group 179
    /// </summary>
    [Map("TRD_GRP_179")]
    TradeGroup179,

    /// <summary>
    /// Trade group 180
    /// </summary>
    [Map("TRD_GRP_180")]
    TradeGroup180,

    /// <summary>
    /// Trade group 181
    /// </summary>
    [Map("TRD_GRP_181")]
    TradeGroup181,

    /// <summary>
    /// Trade group 182
    /// </summary>
    [Map("TRD_GRP_182")]
    TradeGroup182,

    /// <summary>
    /// Trade group 183
    /// </summary>
    [Map("TRD_GRP_183")]
    TradeGroup183,

    /// <summary>
    /// Trade group 184
    /// </summary>
    [Map("TRD_GRP_184")]
    TradeGroup184,

    /// <summary>
    /// Trade group 185
    /// </summary>
    [Map("TRD_GRP_185")]
    TradeGroup185,

    /// <summary>
    /// Trade group 186
    /// </summary>
    [Map("TRD_GRP_186")]
    TradeGroup186,

    /// <summary>
    /// Trade group 187
    /// </summary>
    [Map("TRD_GRP_187")]
    TradeGroup187,

    /// <summary>
    /// Trade group 188
    /// </summary>
    [Map("TRD_GRP_188")]
    TradeGroup188,

    /// <summary>
    /// Trade group 189
    /// </summary>
    [Map("TRD_GRP_189")]
    TradeGroup189,

    /// <summary>
    /// Trade group 190
    /// </summary>
    [Map("TRD_GRP_190")]
    TradeGroup190,

    /// <summary>
    /// Trade group 191
    /// </summary>
    [Map("TRD_GRP_191")]
    TradeGroup191,

    /// <summary>
    /// Trade group 192
    /// </summary>
    [Map("TRD_GRP_192")]
    TradeGroup192,

    /// <summary>
    /// Trade group 193
    /// </summary>
    [Map("TRD_GRP_193")]
    TradeGroup193,

    /// <summary>
    /// Trade group 194
    /// </summary>
    [Map("TRD_GRP_194")]
    TradeGroup194,

    /// <summary>
    /// Trade group 195
    /// </summary>
    [Map("TRD_GRP_195")]
    TradeGroup195,

    /// <summary>
    /// Trade group 196
    /// </summary>
    [Map("TRD_GRP_196")]
    TradeGroup196,

    /// <summary>
    /// Trade group 197
    /// </summary>
    [Map("TRD_GRP_197")]
    TradeGroup197,

    /// <summary>
    /// Trade group 198
    /// </summary>
    [Map("TRD_GRP_198")]
    TradeGroup198,

    /// <summary>
    /// Trade group 199
    /// </summary>
    [Map("TRD_GRP_199")]
    TradeGroup199,

    /// <summary>
    /// Trade group 200
    /// </summary>
    [Map("TRD_GRP_200")]
    TradeGroup200,

    /// <summary>
    /// Trade group 201
    /// </summary>
    [Map("TRD_GRP_201")]
    TradeGroup201,

    /// <summary>
    /// Trade group 202
    /// </summary>
    [Map("TRD_GRP_202")]
    TradeGroup202,

    /// <summary>
    /// Trade group 203
    /// </summary>
    [Map("TRD_GRP_203")]
    TradeGroup203,

    /// <summary>
    /// Trade group 204
    /// </summary>
    [Map("TRD_GRP_204")]
    TradeGroup204,

    /// <summary>
    /// Trade group 205
    /// </summary>
    [Map("TRD_GRP_205")]
    TradeGroup205,

    /// <summary>
    /// Trade group 206
    /// </summary>
    [Map("TRD_GRP_206")]
    TradeGroup206,

    /// <summary>
    /// Trade group 207
    /// </summary>
    [Map("TRD_GRP_207")]
    TradeGroup207,

    /// <summary>
    /// Trade group 208
    /// </summary>
    [Map("TRD_GRP_208")]
    TradeGroup208,

    /// <summary>
    /// Trade group 209
    /// </summary>
    [Map("TRD_GRP_209")]
    TradeGroup209,

    /// <summary>
    /// Trade group 210
    /// </summary>
    [Map("TRD_GRP_210")]
    TradeGroup210,

    /// <summary>
    /// Trade group 211
    /// </summary>
    [Map("TRD_GRP_211")]
    TradeGroup211,

    /// <summary>
    /// Trade group 212
    /// </summary>
    [Map("TRD_GRP_212")]
    TradeGroup212,

    /// <summary>
    /// Trade group 213
    /// </summary>
    [Map("TRD_GRP_213")]
    TradeGroup213,

    /// <summary>
    /// Trade group 214
    /// </summary>
    [Map("TRD_GRP_214")]
    TradeGroup214,

    /// <summary>
    /// Trade group 215
    /// </summary>
    [Map("TRD_GRP_215")]
    TradeGroup215,

    /// <summary>
    /// Trade group 216
    /// </summary>
    [Map("TRD_GRP_216")]
    TradeGroup216,

    /// <summary>
    /// Trade group 217
    /// </summary>
    [Map("TRD_GRP_217")]
    TradeGroup217,

    /// <summary>
    /// Trade group 218
    /// </summary>
    [Map("TRD_GRP_218")]
    TradeGroup218,

    /// <summary>
    /// Trade group 219
    /// </summary>
    [Map("TRD_GRP_219")]
    TradeGroup219,

    /// <summary>
    /// Trade group 220
    /// </summary>
    [Map("TRD_GRP_220")]
    TradeGroup220,

    /// <summary>
    /// Trade group 221
    /// </summary>
    [Map("TRD_GRP_221")]
    TradeGroup221,

    /// <summary>
    /// Trade group 222
    /// </summary>
    [Map("TRD_GRP_222")]
    TradeGroup222,

    /// <summary>
    /// Trade group 223
    /// </summary>
    [Map("TRD_GRP_223")]
    TradeGroup223,

    /// <summary>
    /// Trade group 224
    /// </summary>
    [Map("TRD_GRP_224")]
    TradeGroup224,

    /// <summary>
    /// Trade group 225
    /// </summary>
    [Map("TRD_GRP_225")]
    TradeGroup225,

    /// <summary>
    /// Trade group 226
    /// </summary>
    [Map("TRD_GRP_226")]
    TradeGroup226,

    /// <summary>
    /// Trade group 227
    /// </summary>
    [Map("TRD_GRP_227")]
    TradeGroup227,

    /// <summary>
    /// Trade group 228
    /// </summary>
    [Map("TRD_GRP_228")]
    TradeGroup228,

    /// <summary>
    /// Trade group 229
    /// </summary>
    [Map("TRD_GRP_229")]
    TradeGroup229,

    /// <summary>
    /// Trade group 230
    /// </summary>
    [Map("TRD_GRP_230")]
    TradeGroup230,

    /// <summary>
    /// Trade group 231
    /// </summary>
    [Map("TRD_GRP_231")]
    TradeGroup231,

    /// <summary>
    /// Trade group 232
    /// </summary>
    [Map("TRD_GRP_232")]
    TradeGroup232,

    /// <summary>
    /// Trade group 233
    /// </summary>
    [Map("TRD_GRP_233")]
    TradeGroup233,

    /// <summary>
    /// Trade group 234
    /// </summary>
    [Map("TRD_GRP_234")]
    TradeGroup234,

    /// <summary>
    /// Trade group 235
    /// </summary>
    [Map("TRD_GRP_235")]
    TradeGroup235,

    /// <summary>
    /// Trade group 236
    /// </summary>
    [Map("TRD_GRP_236")]
    TradeGroup236,

    /// <summary>
    /// Trade group 237
    /// </summary>
    [Map("TRD_GRP_237")]
    TradeGroup237,

    /// <summary>
    /// Trade group 238
    /// </summary>
    [Map("TRD_GRP_238")]
    TradeGroup238,

    /// <summary>
    /// Trade group 239
    /// </summary>
    [Map("TRD_GRP_239")]
    TradeGroup239,

    /// <summary>
    /// Trade group 240
    /// </summary>
    [Map("TRD_GRP_240")]
    TradeGroup240,

    /// <summary>
    /// Trade group 241
    /// </summary>
    [Map("TRD_GRP_241")]
    TradeGroup241,

    /// <summary>
    /// Trade group 242
    /// </summary>
    [Map("TRD_GRP_242")]
    TradeGroup242,

    /// <summary>
    /// Trade group 243
    /// </summary>
    [Map("TRD_GRP_243")]
    TradeGroup243,

    /// <summary>
    /// Trade group 244
    /// </summary>
    [Map("TRD_GRP_244")]
    TradeGroup244,

    /// <summary>
    /// Trade group 245
    /// </summary>
    [Map("TRD_GRP_245")]
    TradeGroup245,

    /// <summary>
    /// Trade group 246
    /// </summary>
    [Map("TRD_GRP_246")]
    TradeGroup246,

    /// <summary>
    /// Trade group 247
    /// </summary>
    [Map("TRD_GRP_247")]
    TradeGroup247,

    /// <summary>
    /// Trade group 248
    /// </summary>
    [Map("TRD_GRP_248")]
    TradeGroup248,

    /// <summary>
    /// Trade group 249
    /// </summary>
    [Map("TRD_GRP_249")]
    TradeGroup249,

    /// <summary>
    /// Trade group 250
    /// </summary>
    [Map("TRD_GRP_250")]
    TradeGroup250,

    /// <summary>
    /// Trade group 251
    /// </summary>
    [Map("TRD_GRP_251")]
    TradeGroup251,

    /// <summary>
    /// Trade group 252
    /// </summary>
    [Map("TRD_GRP_252")]
    TradeGroup252,

    /// <summary>
    /// Trade group 253
    /// </summary>
    [Map("TRD_GRP_253")]
    TradeGroup253,

    /// <summary>
    /// Trade group 254
    /// </summary>
    [Map("TRD_GRP_254")]
    TradeGroup254,

    /// <summary>
    /// Trade group 255
    /// </summary>
    [Map("TRD_GRP_255")]
    TradeGroup255,

    /// <summary>
    /// Trade group 256
    /// </summary>
    [Map("TRD_GRP_256")]
    TradeGroup256,

    /// <summary>
    /// Trade group 257
    /// </summary>
    [Map("TRD_GRP_257")]
    TradeGroup257,

    /// <summary>
    /// Trade group 258
    /// </summary>
    [Map("TRD_GRP_258")]
    TradeGroup258,

    /// <summary>
    /// Trade group 259
    /// </summary>
    [Map("TRD_GRP_259")]
    TradeGroup259,

    /// <summary>
    /// Trade group 260
    /// </summary>
    [Map("TRD_GRP_260")]
    TradeGroup260,

    /// <summary>
    /// Trade group 261
    /// </summary>
    [Map("TRD_GRP_261")]
    TradeGroup261,

    /// <summary>
    /// Trade group 262
    /// </summary>
    [Map("TRD_GRP_262")]
    TradeGroup262,

    /// <summary>
    /// Trade group 263
    /// </summary>
    [Map("TRD_GRP_263")]
    TradeGroup263,

    /// <summary>
    /// Trade group 264
    /// </summary>
    [Map("TRD_GRP_264")]
    TradeGroup264,

    /// <summary>
    /// Trade group 265
    /// </summary>
    [Map("TRD_GRP_265")]
    TradeGroup265,

    /// <summary>
    /// Trade group 266
    /// </summary>
    [Map("TRD_GRP_266")]
    TradeGroup266,

    /// <summary>
    /// Trade group 267
    /// </summary>
    [Map("TRD_GRP_267")]
    TradeGroup267,

    /// <summary>
    /// Trade group 268
    /// </summary>
    [Map("TRD_GRP_268")]
    TradeGroup268,

    /// <summary>
    /// Trade group 269
    /// </summary>
    [Map("TRD_GRP_269")]
    TradeGroup269,

    /// <summary>
    /// Trade group 270
    /// </summary>
    [Map("TRD_GRP_270")]
    TradeGroup270,

    /// <summary>
    /// Trade group 271
    /// </summary>
    [Map("TRD_GRP_271")]
    TradeGroup271,

    /// <summary>
    /// Trade group 272
    /// </summary>
    [Map("TRD_GRP_272")]
    TradeGroup272,

    /// <summary>
    /// Trade group 273
    /// </summary>
    [Map("TRD_GRP_273")]
    TradeGroup273,

    /// <summary>
    /// Trade group 274
    /// </summary>
    [Map("TRD_GRP_274")]
    TradeGroup274,

    /// <summary>
    /// Trade group 275
    /// </summary>
    [Map("TRD_GRP_275")]
    TradeGroup275,

    /// <summary>
    /// Trade group 276
    /// </summary>
    [Map("TRD_GRP_276")]
    TradeGroup276,

    /// <summary>
    /// Trade group 277
    /// </summary>
    [Map("TRD_GRP_277")]
    TradeGroup277,

    /// <summary>
    /// Trade group 278
    /// </summary>
    [Map("TRD_GRP_278")]
    TradeGroup278,

    /// <summary>
    /// Trade group 279
    /// </summary>
    [Map("TRD_GRP_279")]
    TradeGroup279,

    /// <summary>
    /// Trade group 280
    /// </summary>
    [Map("TRD_GRP_280")]
    TradeGroup280,

    /// <summary>
    /// Trade group 281
    /// </summary>
    [Map("TRD_GRP_281")]
    TradeGroup281,

    /// <summary>
    /// Trade group 282
    /// </summary>
    [Map("TRD_GRP_282")]
    TradeGroup282,

    /// <summary>
    /// Trade group 283
    /// </summary>
    [Map("TRD_GRP_283")]
    TradeGroup283,

    /// <summary>
    /// Trade group 284
    /// </summary>
    [Map("TRD_GRP_284")]
    TradeGroup284,

    /// <summary>
    /// Trade group 285
    /// </summary>
    [Map("TRD_GRP_285")]
    TradeGroup285,

    /// <summary>
    /// Trade group 286
    /// </summary>
    [Map("TRD_GRP_286")]
    TradeGroup286,

    /// <summary>
    /// Trade group 287
    /// </summary>
    [Map("TRD_GRP_287")]
    TradeGroup287,

    /// <summary>
    /// Trade group 288
    /// </summary>
    [Map("TRD_GRP_288")]
    TradeGroup288,

    /// <summary>
    /// Trade group 289
    /// </summary>
    [Map("TRD_GRP_289")]
    TradeGroup289,

    /// <summary>
    /// Trade group 290
    /// </summary>
    [Map("TRD_GRP_290")]
    TradeGroup290,

    /// <summary>
    /// Trade group 291
    /// </summary>
    [Map("TRD_GRP_291")]
    TradeGroup291,

    /// <summary>
    /// Trade group 292
    /// </summary>
    [Map("TRD_GRP_292")]
    TradeGroup292,

    /// <summary>
    /// Trade group 293
    /// </summary>
    [Map("TRD_GRP_293")]
    TradeGroup293,

    /// <summary>
    /// Trade group 294
    /// </summary>
    [Map("TRD_GRP_294")]
    TradeGroup294,

    /// <summary>
    /// Trade group 295
    /// </summary>
    [Map("TRD_GRP_295")]
    TradeGroup295,

    /// <summary>
    /// Trade group 296
    /// </summary>
    [Map("TRD_GRP_296")]
    TradeGroup296,

    /// <summary>
    /// Trade group 297
    /// </summary>
    [Map("TRD_GRP_297")]
    TradeGroup297,

    /// <summary>
    /// Trade group 298
    /// </summary>
    [Map("TRD_GRP_298")]
    TradeGroup298,

    /// <summary>
    /// Trade group 299
    /// </summary>
    [Map("TRD_GRP_299")]
    TradeGroup299,

    /// <summary>
    /// Trade group 300
    /// </summary>
    [Map("TRD_GRP_300")]
    TradeGroup300,
}
