import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
import {AutentifikacijaHelper} from "../_helpers/autentifikacija-helper";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  title:string = 'angularFIT2';
  ime_prezime:string = '';
  opstina: string = '';
  studentPodaci: any;
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  prikaziAdd = false;
  prikaziEdit = false;
  defaultOpstina : string;
  opstine: any;
  studentId:number;
  ime:string;
  prezime:string;

  constructor(private httpKlijent: HttpClient, private router: Router) {
  }

  testirajWebApi() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAll", MojConfig.http_opcije()).subscribe(x=>{
      this.studentPodaci = x;
    });
  }

  ngOnInit(): void {
    this.testirajWebApi();
  }

  filterStudenti()
  {
    if(this.ime_prezime=='' && this.opstina=='')
      return this.studentPodaci;
    return this.studentPodaci.filter(
      (x:any) => (x.ime + " " + x.prezime).toLowerCase().startsWith(this.ime_prezime.toLowerCase()) ||
        (x.prezime + " " + x.ime).toLowerCase().startsWith(this.ime_prezime.toLowerCase()) &&
        (x.opstina_rodjenja.description.toLowerCase().startsWith(this.opstina.toLowerCase()))
    )
  }

  getOpstine()
  {
    this.httpKlijent.get(MojConfig.adresa_servera+"/Opstina/GetByAll", MojConfig.http_opcije()).subscribe(
      (opstine:any) =>
      {
        this.opstine = opstine;
        for (let i = 0; i<opstine.length;i++)
        {
            let defaultna = opstine[0];
            if(opstine[i].id > defaultna.id)
              this.defaultOpstina= opstine[i].opis;
        }
      }
    );
  }

  otvaranjeAdd($event : boolean)
  {
    this.prikaziAdd = $event;
  }
  otvaranjeEdit($event : boolean)
  {
    this.prikaziEdit = $event;
  }

  pripremiNovogStudenta()
  {
    this.getOpstine();
    this.ime=this.ime_prezime.split(' ')[0][0].toUpperCase()+
      this.ime_prezime.split(' ')[0].replace(this.ime_prezime.split(' ')[0][0],'');

    this.prezime=this.ime_prezime.split(' ')[1][0].toUpperCase()+
      this.ime_prezime.split(' ')[1].replace(this.ime_prezime.split(' ')[1][0],'');
  }

  pripremiStudentaEdit(id:number)
  {
    this.getOpstine();
    let studentId = new HttpParams();
    studentId = studentId.append('studentId', id);
    this.httpKlijent.get(MojConfig.adresa_servera+"/GetStudentById", {
      params:studentId,
      observe: "response",
      headers :
        {
          'autentifikacija-token':AutentifikacijaHelper.getLoginInfo().autentifikacijaToken.vrijednost
        }
    }).subscribe(
      response =>
      {
        if(response.status ===200)
        {
          let student = JSON.parse(JSON.stringify(response.body));
          this.ime = student.ime;
          this.prezime = student.prezime;
          this.studentId = id;
          this.opstine.forEach((opstina:any)=>{
            if (opstina.id==student.opstina_rodjenja_id)
              this.defaultOpstina=opstina.opis;
          })
        }
      }
    );
  }

  obrisiStudenta(id:number)
  {
    let studentId = new HttpParams();
    studentId = studentId.append('studentId', id);
    this.httpKlijent.delete(MojConfig.adresa_servera+"/ObrisiStudenta", {
      params:studentId,
      headers:
        {
          'autentifikacija-token': AutentifikacijaHelper.getLoginInfo().autentifikacijaToken.vrijednost
        }
    }).subscribe(
      () => {
        porukaSuccess("Uspjesno obrisan student!");
        window.location.reload();
      }
    );
  }

}
