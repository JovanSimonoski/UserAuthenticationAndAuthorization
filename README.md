*Оваа веб страна има за цел да покаже имплементација на регистрација,
 автентикација, авторизација, промена на лозинка и обновување на лозинка доколку истата е
 заборавена.

 Доколку не сме најавени на страницата ги имаме следните можности:
   - Регистрација
     - За да се регистрираме на страната потребно е да бидат внесени email, лозинка и
     да се потври лозинката. Најпрво, на самиот frontend се проверува дали лозинката која
     сакаме да ја внесеме е доволно добра во однос на должина, специјални карактери,
     големи и мали букви и бројки и според тоа се прикажува колку е добра лозинката (weak,
     moderate, strong или very strong). Исто така се врши проверка дали се совраѓаат
     лозинката и потврдената лозинка и доколку не се совпаѓаат немаме можност да се
     регистрираме или доколку лозинката не е барем со moderate јачина.
     - Откако ќе ги исполниме овие услови, може да се испрати барањето до backend-от,
     но пред да се испрати истото, се хешира лозинката на frontend со цел да биде зашититена
     при преносот. За хеширање на frontend се користи sha-256.
     Кога барањето ќе пристигне до backend-от (POST Request), најпрво се проверува
     дали веќе постои профил со истиот емаил и ако да, се враќа соодветна порака. Доколку
     се е во ред, лозинката повторно се хешира, но овој пат дополнително се додава и salt и
     овој salt заедно со хешираната лозинка се зачувуваат во база.
     - Следно корисникот добива емаил порака на во која се наоѓа код за потврдување на
     емаил адресата и е пренасочен на страна каде што треба да го внесе овој код. Кодот се
     состои од 5 случајно избрани цифри и трае само 5 минути. По истекување на времето
     истиот е невалиден. При секој обид за потврдување на емаил адресата се генерира нов
     код со цел да се постигне подобра безбедност. При генерирање на код, тој се зачувува во
     табелата на корисниците кај соодветниот корисник и дополнително се зачувува и времето
     до кога е валиден.
     - Откако ќе го внесеме кодот, доколку е валиден и точен веќе сме најавени на
     страницата и овој код се брише.
     - Доколку внесеме неточен или невалиден код, добиваме повторна шанса за
     внесување но овој пат добиваме и нов код.
  - Најава
     - За да се најавиме на страницата потребно е да внесиме емаил адреса и лозинка.
     Лозинката се хешира на frontend со sha-256 и податоците се испраќаат до backend-от со
     помош на POST Request за подобра безбедност.
     - Откако барањето ќе стигне до backend-от, се проверува емаил адресата и доколку
     не постои корисник со таа емаил адреса се враќа соодветна порака. Доколку емаил
     адресата е валидна, се хешира внесената лозинка во комбинација со salt-от кој бил
     искористен за хеширање на оригиналната лозинка и се споредуваат двете лозинки.
     - Доколку лозинките не се совпаѓаат се враќа соодветна порака. Доколку лозинките се
     совпаѓаат, се испраќа код за двофакторска автентикација на емаил-от на корисникот и
     овој код слично како и кодот за потврдување на емаил адресата трае 5 минути и истиот е
     невалиден по истекување на времето (се работи за различен код, не истиот како за
     потврдување на адресата). Доколку корисникот го внесе точниот код, се најавува на веб
     страната и кодот се брише, а доколку кодот што го внел е невалиден или погрешен
     повторно се испраќа нов код.
     *Доколку корисникот при најава ги внесе точниот емаил и лозинка, но претходно
     при регистрација на го потврдил својот емаил, повторно добива код за потврдување на
     емаилот а не код за двофакторска автентикација. Откако ќе го потврди емаилот,
     корисникот ќе биде автоматски најавен.
  - Заборавена лозинка / Обновување на лозинка
     - Доколку корисникот ја заборавил својата лозинка, со клик на "Forgot Password" има
     можност да ја внесе својата емаил адреса на која ќе добие код за обновување на
     лозинката. Овој код е различен од претходните два кои ги споменавме, но работи на
     потполно истиот принцип. Доклку се внесе валиден и топчен код од страна на корисникот,
     истиот добива право да си ја обнови лозинката со внесување на нова лозинка и
     потврдување на истата. Тука важат истите контроли и правила за лозинката како и при
     регистрација на корисникот. Од тука повторно лозинката се испраќа хеширана и истата
     повторно се хешира на backend но овој пат дополнително се додава и salt и се прави
     соодвена промена во базата.

     Доколку сме најавени на страницата ги имаме следните можности:

  - Одјава
     - Едноставен метод којшто само го одјавува корисникот и прави соодветни промени
     во сесијата за да се означи неговото одјавување.
  - Промена на лозинка
     - При промена на лозинката треба да ја внесеме старата лозинка и да внесеме нова
     лозинка како и да ја потврдиме новата лозинка. За новата лозинка важат истите правила и
     контроли како и при регистрација на нов корисник, односно се прави проверка на јачината
     на лозинката и дали истата се совпаѓа со потврдувањето на лозинката. Откако ќе се
     внесат потребните лозинки, истите се хешираат на frontend со sha-256 и се испраќаат
     преку POST Request до backend-от. Таму повторно се хешира внесената стара лозинка
     придружена со оригиналниот salt кој бил искористен при хеширање на оригиналната
     лозинка и истата се споредува со оригиналната хеширана лозинка за да се осигураме
     дека лозинките се совпаѓаат. Доколку не се совпаѓаат добиваме соодветна порака, а
     доколку проверката помине, се хешира новата лозинка со нов salt и се прави соодветна
     промена во базата, односно се зачувуваат новата хеширана лозинка и новиот salt.
    
 * Дополнително, во позадина веб страната има посебна табела во која се зачувуваат
 логови од обиди за најавување, односно за секој обид се чува време, емаил и дали истиот
 бил успешен со цел да може да се прават најразлични безбедносни анализи.
 Од технички аспект се користат повеќе помошни модели кои служат за правилно
 работење на страната, но единствените податоци кои се зачувуваат во база се податоци
 за корисниците: email, salt, хеширана лозинка со помош на salt-от, еден код за
 регистрација, еден за двофакторска автентикација и еден за заборавена лозинка и тоа
 полињата за овие кодови се пополнети само во текот на тие 5 минути додека кодот е
 валиден и истите се пребришуваат после употребувањето на кодот. Покрај кодовите се
 чува и време како информација до кога истите се валидни. И времето кое се чува исто
 така има валиден формат само додека е активен кодот а по искористување на истиот,
 времето се поставува на претходниот ден (со цел да не е валидно).
 Хеширањето на frontend и backend се остварува со различни алгоритми со цел да не му се
 овозможи на напаѓачите да го дознаат алгоритмот со кој се хешира. Лозинките постојано
 се чуваат хеширани во база, па дури и се пренесуваат хеширани од frontend-от до
 backend-от каде што повторно се хешираат но овој пат со salt за да се овозможи дури и
 кога два или повеќе кориници имаат исти лозинки, во базата да се зачувуваат како
 различни.
 Испраќањето на емаил пораки со кодови се изведува преку SMTP сервер, во случајов
 Gmail.
 Сите POST Request-и кои се користат за пренесување на податоци помеѓу
 методите/акциите се енкриптирани. Ова е овозможено од самиот framework во кој
 работев.

 Во рамките на оваа страница постојат 3 улоги на корисници:
   - Admin
     - Овојтип на корисници ги поседуваат сите права на страницата. Корисници
     од тип Admin се многу ретки и истите имаат можност да едитираат и бришат
     други корисници од страната во случај кога тоа е потребно. Овие корисници
     имаат поинаков изглед на нивната “домашна” страна преку која може да
     навигираат до специјалните акции кои им се овозможени како на пример
     преглед на сите корисници и информации за истите или додавање на некој
     корисник кон одредена улога/role.
   - Moderator
     - Овојтип на корисници имаат можност да ја гледаат листата на корисници и
     единствено можат да прават промени врз корисничките профили доколку
     тоа е потребно, но немаат можност да бришат кориснички профили како
     што беше случај кај администраторите.
   - User
     - Овојтип на корисници ги имаат најмалку од привилегиите, односно ова се
     обични корисници на страната кои имаат можност да работаат на страната,
     да прават промени врз својот профил и слично, но немаат можност да
     прегледуваат информации за останатите учесници а најмалку да прават
     промени врз нивните профили или да бришат туѓи профили.
     
 Доколку не сме најавени на страната немаме пристап до услугите кои ги нуди оваа
 страна, единствено може да се регистрираме, да го обновиме нашиот кориснички профил
 при заборавена лозинка или да се најавиме.
 Во зависност од улогата на најавениот корисник се прикажува поинаква содржина
 на страницата. Постојат копчиња кои се појавуваат доколку сме најавени како
 администратор или модератор а истите не се достапни доколку сме најавени како обичен
 корисник или доколку воопшто не сме најавени на странта. Дополнително и во самиот код
 на backend-от има воведени рестрикции за улогата на коринсникот за во случај даден
 корисник да се обиде да модифицира барање го серверот за да ги зголеми своите
 привилегии односно да пристапи до акции кои му се забранети/оневозможени.
 Како дополнителни функционалности на страната се додадени додавањето на
 улога на даден корисник која функционалност му е овозможена само на
 администраторите. Тие може да го наведат email-от на корисникот и од drop-down листа
 да ја изберат улогата која сакаат да му биде доделена. Дополнително е додадена
 функционалност преку која може да се прават промени врз корисничките профили од
 страна на модераторите и администраторите доколку истото е потребно и додадена е
 можност за бришење на кориснички профил која можност е овозможена само за
 администраторите.
 Има ипосебна секција на страната на која администраторите и модераторите може
 да гледаат листа од сите корисници на страната и податоците за истите.
