#include <iostream>
#include <string>
#include <math.h>
#include <fstream>

#define base 256
#define P 1000000007
using namespace std;

class KarpRabin{
public:
    void Search(string input, string pattern){
        unsigned long N = pattern.length();
        unsigned long T = input.length();
        long long H = 0;
        long long H0 = 0;
        int hashed_base = base%P;
        for(int i = 0;i<N;i++){
            if(i<N-1){
                H = ((H+input[i])*hashed_base)%P;
                H0 = ((H0+pattern[i])*hashed_base)%P;
            }
            else{
                H = H+input[i];
                H0 = H0+pattern[i];
            }
        }
        for(int i = 0;i<T-N+1;i++){
            if(H == H0){
                for(int k = 0;k<N;k++){
                    if(pattern[k]!=input[k]){
                        break;
                    }
                    else if (k==N-1){
                        cout<<i<<" ";
                    }
                }
            }
            H = Replacing(H, input[i], input[i+N],N);
        }
    }
    long long Replacing(long long Hi,int v,int v_n, unsigned long N){
        long long H = 0;
        int n1 = (v*pow(base,N-1));
        n1 = n1%P;
        H = ((((Hi-n1)+P)*base)%P)+v_n;
        return H;
    }
};
int main(int argc, const char * argv[]) {
    KarpRabin kr;
    string file = " ";
    int lp;
    cin>>lp;
    for(int i = 0;i < lp;i++){
        cin>>file;
        ifstream myfile(file);
        string input = "";
        string pattern = "";
        cin>>pattern;
        if (myfile){
            getline(myfile, input);
            myfile.close();
            }
        
        kr.Search(input, pattern);
    }
    return 0;
}
