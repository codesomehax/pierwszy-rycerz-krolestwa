#include <iostream>
#include <string>
#include <fstream>

#define base 256
#define P 1000000007
using namespace std;

long long Replacing(long long Hi,int v,int v_n, unsigned long power){
    long long H = 0;
    H = (((Hi+P-v*power%P)*base)+v_n)%P;
    return H;
}
void Search(string input, string pattern){
    unsigned long N = pattern.length();
    unsigned long T = input.length();
    //cout<<T<<endl;
    string temp;
    long long H = 0;
    long long H0 = 0;
    long long power = 1;
    for(int i = 0;i<N-1;i++){
        power = (power*base)%P;
    }
    for(int i = 0;i<N;i++){
        if(i<N-1){
            H = ((H+input[i])*base)%P;
            H0 = ((H0+pattern[i])*base)%P;
        }
        else{
            H = H+input[i];
            H0 = H0+pattern[i];
        }
    }
    for(int i = 0;i<T-N+1;i++){
        //cout<<"iteration: "<<i<<" H:"<<H<<" H0:"<<H0<<endl;
        if(H == H0){
            bool alike = true;
            for(int j = 0;j<N;j++){
                //cout<<input[i+j]<<endl;
                //cout<<pattern[j]<<endl;
                if(pattern[j]!=input[i+j]){
                    alike = false;
                    //break;
                }
            }
            if(alike == true){
                cout<<i<<" ";
            }
            //cout<<endl;
        }
        //cout<<"out:"<<input[i]<<" in:"<<input[i+N]<<endl;
        H = Replacing(H, input[i], input[i+N],power);
    }
}


int main() {
    /*string file = " ";
    int lp;
    cin>>lp;
    for(int i = 0;i < lp;i++){
        cin>>file;
        ifstream myfile(file);*/
        //string input = "EEE.4EEOOOOOEEEE.2EEOOOASDsaOOOOEE.4EAAAEEE.4E";
        //string pattern = "EE.4";
        /*cin>>pattern;
        if (myfile){
            getline(myfile, input);
            myfile.close();
            }
        if(input.length()<=0){
            return 0;
        }*/
    int lp;
    ifstream inFile;
    string input;
    string file;
    string pattern = "";
    cin>>lp;
    if(lp<0){
        cout<<"nieprawidłowa liczba przypadków"<<endl;
    }
    for(int i = 0;i < lp;i++){
        cin>>file;
        inFile.open(file);
        //inFile.open("/Users/Thomas/Desktop/AiDS2/K-R/KR-files/input/text.txt");
        getline(inFile, input);
        inFile.close();
        cin>>pattern;
        //input="3.1415.15";
        //pattern=".";
        Search(input, pattern);
        cout<<endl;
    }
    return 0;
}
