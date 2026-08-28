using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMachine
{
    public enum eCommand
    {
		None = 0,
		VER	 =01,		//　		    @버전				A/M		현재 FIRMWARE 버젼확인
		AGN	 =02,		//0		    @AGN / @ERR			A		웨이퍼 얼라인 실행
		RST	 =03,		//　		    @RST				A/M		에러 리셋
		ERR	 =04,		//　		    에러코드(ex @E01)   	A/M		에러 정보 호출
		HOM	 =05,		//　		    @HOM				A		MOTOR홈위치 이동
		VON	 =06,		//　		    @VON				M		VACUUM ON
		VOF	 =07,		//　		    @VOF				M		VACUUM OFF
		TRR	 =08,		//0~3.00	@TRR				A/M		척을 CW방향으로 입력된 Argument 만큼 돌린다
		TLL	 =09,		//0~3.00	@TLL				A/M		척을 CCW방향으로 입력된 Argument 만큼 돌린다..
		AUT	 =10,		//			@AUT				A/M		얼라이너를 자동운전 모드로 바꾼다.
		MAN	 =11,		//			@MAN				A/M		얼라이너를 수동운전 모드로 바꾼다.
		STA	 =12,		//			@현재 상태			A/M		현재 얼라이너의 상태를 요청한다.
		WCK	 =13,		//			@EXT / @NOT			A/M		웨이퍼에 유무를 감지 한다.
		AYR	 =14,		//			@AYR / @ERR			A		Wafer 받을 준비인지 확인한다.
		RCP	 =15,		//			@RCP				A		Wafer 투입 후 알맞은 Recipe를 적용한다.
		INT	 =16,		//			@INT /@ERR			M		Motor들을 Home Init.합니다.
		BCR	 =17		    //			@BCR /@ERR			A/M		
    }
}
