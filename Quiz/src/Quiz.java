import java.security.MessageDigest;
import java.math.BigInteger;
import java.util.Arrays;

public class Quiz {
    public static void main(String[] args) throws Exception {
        MessageDigest md = MessageDigest.getInstance("MD5");
        byte[] digest = md.digest("abracadabra".getBytes("UTF-8"));
        for (byte b : digest) {
            System.out.printf("%02x", b);
        }

        System.out.println(booleanExpression(true,false,false,false));
        System.out.println(0x0bp3);
        System.out.println(flipBit(13,1));

        System.out.println(isPowerOfTwo(1));

        System.out.println(Arrays.toString(mergeArrays(new int[]{1,2},new int[]{0,1,1})));
    }

    public static boolean booleanExpression(boolean a, boolean b, boolean c, boolean d) {
        int s=0;
        s+=a?1:0;
        s+=b?1:0;
        s+=c?1:0;
        s+=d?1:0;

        return s==2;
    }
    /**
     * Flips one bit of the given <code>value</code>.
     *
     * @param value     any number
     * @param bitIndex  index of the bit to flip, 1 <= bitIndex <= 32
     * @return new value with one bit flipped
     */
    public static int flipBit(int value, int bitIndex) {
        int v=(int)Math.pow(2,bitIndex-1);
        return value^v; // put your implementation here
    }

    /**
     * Checks if given <code>value</code> is a power of two.
     *
     * @param value any number
     * @return <code>true</code> when <code>value</code> is power of two, <code>false</code> otherwise
     */
    public static boolean isPowerOfTwo(int value) {
        value=Math.abs(value);
        if(value==0) return false;
        return (value&(value-1))==0;
    }

    /**
     * Checks if given <code>text</code> is a palindrome.
     *
     * @param text any string
     * @return <code>true</code> when <code>text</code> is a palindrome, <code>false</code> otherwise
     */
    public static boolean isPalindrome(String text) {

       var txt=text.replaceAll("[^A-Za-z1-9]+", "").toLowerCase().toCharArray();
for(int i=0;i<txt.length/2;i++)
    if(txt[i]!=txt[txt.length-1-i])
        return  false;

        return true; // your implementation here
    }

    /**
     * Calculates factorial of given <code>value</code>.
     *
     * @param value positive number
     * @return factorial of <code>value</code>
     */
    public static BigInteger factorial(int value) {
        if(value==1) return BigInteger.ONE;
        return  BigInteger.valueOf(value).multiply(factorial(value-1)); // your implementation here
    }


    /**
     * Merges two given sorted arrays into one
     *
     * @param a1 first sorted array
     * @param a2 second sorted array
     * @return new array containing all elements from a1 and a2, sorted
     */
    public static int[] mergeArrays(int[] a1, int[] a2) {
    if(a1.length==0) return  a2;
    if(a2.length==0) return a1;

        int[] res=new int[a1.length+a2.length];
        int i1=0,i2=0,i=0;
        for(;i<res.length;i++)
        {
            if(a1[i1]<a2[i2])
                res[i]=a1[i1++];
            else
                res[i]=a2[i2++];

            if(i1==a1.length||i2==a2.length)
                break;
        }
        i++;
        if(i1==a1.length){
            for(;i<res.length;i++)
                res[i]=a2[i2++];
        }
            else
                for(;i<res.length;i++)
            res[i]=a1[i1++];


        return res; // your implementation here
    }











}