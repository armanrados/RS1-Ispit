import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient, HttpParams} from "@angular/common/http";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css']
})
export class StudentMaticnaknjigaComponent implements OnInit {
  maticnaKnjiga: any;
  id: number;
  sub: any;
  godine: any;
  semestar: any = null;
  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}


  ngOnInit(): void {
    this.loadGodine();
    this.sub = this.route.params.subscribe((res:any) => {
      this.id = +res["id"];
      this.loadMaticna();
    })
  }
  loadMaticna(){
    this.httpKlijent.get(MojConfig.adresa_servera + "/GetMaticna?id="+ this.id, MojConfig.http_opcije())
      .subscribe((res:any) => {
        this.maticnaKnjiga = res;
      })
  }
  loadGodine(){
    this.httpKlijent.get(MojConfig.adresa_servera + "/GetGodine", MojConfig.http_opcije())
      .subscribe((res:any) => {
        this.godine = res;
      })
  }
  zimskiSemestar(){
    this.semestar = {
      id: this.id,
      datum: new Date(),
      godinaStudija: 1,
      akGodina: 1,
      cijenaSkolarine: 0,
      obnova: false
    }
  }

  upisiZimski(){
    this.httpKlijent.post(MojConfig.adresa_servera + "/MaticnaKnjiga/UpisiZimski/" + this.id, this.semestar, MojConfig.http_opcije())
      .subscribe((res:any) => {
        porukaSuccess(`Uspjesno upisan zimski semestar`);
        this.semestar=null;
        window.location.reload();
      },
        error => {
            porukaError("Godina studija je vec dodana za datog studenta! Dodavanje istih godina je moguce samo ukoliko je obnova oznacena");
        })
  }

  ovjeriZimski(id: any) {
    this.httpKlijent.post(MojConfig.adresa_servera + "/MaticnaKnjiga/OvjeriZimski/" + id, MojConfig.http_opcije())
      .subscribe((res:any) => {
        porukaSuccess(`Uspjesno ovjeren zimski semestar`);
        window.location.reload();
      })
  }

}
